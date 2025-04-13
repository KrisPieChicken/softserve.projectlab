using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;
using API.Data.Entities;
using API.Models;
using API.Data;
using API.Models.Logistics;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;

namespace API.Implementations.Domain
{
    public class WarehouseDomain
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public WarehouseDomain(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<Warehouse>> GetWarehouseByIdAsync(int warehouseId)
        {
            try
            {
                var entity = await _context.WarehouseEntities
                  .Where(w => !w.IsDeleted)
                  .Include(w => w.WarehouseItemEntities)
                  .ThenInclude(wi => wi.SkuNavigation) // Include the navigation property
                  .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId);


                if (entity == null)
                    return Result<Warehouse>.Failure("Warehouse not found", 404);

                var warehouse = _mapper.Map<Warehouse>(entity);
                return Result<Warehouse>.Success(warehouse);
            }
            catch (Exception ex)
            {
                return Result<Warehouse>.Failure(
                    $"Failed to retrieve warehouse: {ex.Message}",
                    500,
                    ex.StackTrace);
            }
        }

        public async Task<Result<List<Warehouse>>> GetAllWarehousesAsync()
        {
            try
            {
                var entities = await _context.WarehouseEntities
                    .Where(w => !w.IsDeleted) 
                    .Include(w => w.Branch)
                    .Include(w => w.WarehouseItemEntities)
                    .ThenInclude(wi => wi.SkuNavigation)
                    .ToListAsync();

                return Result<List<Warehouse>>.Success(_mapper.Map<List<Warehouse>>(entities));
            }
            catch (Exception ex)
            {
                return Result<List<Warehouse>>.Failure(
                    $"Failed to retrieve warehouses: {ex.Message}",
                    500,
                    ex.StackTrace);
            }
        }


        public async Task<Result<bool>> AddItemToWarehouseAsync(int warehouseId, AddItemToWarehouseDTO itemDto)
        {
            // Check if the item exists in the system
            var existingItem = await _context.ItemEntities
                .FirstOrDefaultAsync(i => i.Sku == itemDto.Sku);

            if (existingItem == null)
            {
                return Result<bool>.Failure($"Item with SKU {itemDto.Sku} does not exist in the system. Please create the item first.");
            }

            // Check if the warehouse exists
            var warehouse = await _context.WarehouseEntities
                .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId);

            if (warehouse == null)
            {
                return Result<bool>.Failure($"Warehouse with ID {warehouseId} does not exist.");
            }

            // Check if the item is already linked to the warehouse
            var warehouseItem = await _context.WarehouseItemEntities
                .FirstOrDefaultAsync(wi => wi.WarehouseId == warehouseId && wi.Sku == itemDto.Sku);

            if (warehouseItem != null)
            {
                return Result<bool>.Failure($"Item with SKU {itemDto.Sku} is already linked to Warehouse ID {warehouseId}.");
            }

            // Link the item to the warehouse
            var newWarehouseItem = new WarehouseItemEntity
            {
                WarehouseId = warehouseId,
                Sku = existingItem.Sku,
                ItemQuantity = 0 // Default quantity to 0
            };

            _context.WarehouseItemEntities.Add(newWarehouseItem);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Result<bool>.Success(true);
        }



        public async Task<Result<Warehouse>> RemoveItemFromWarehouseAsync(int warehouseId, int itemId)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var warehouseResult = await GetWarehouseByIdAsync(warehouseId);
                if (!warehouseResult.IsSuccess)
                    return warehouseResult;

                var warehouseItem = await _context.WarehouseItemEntities
                    .Include(wi => wi.Sku)
                    .FirstOrDefaultAsync(wi => wi.WarehouseId == warehouseId && wi.Sku == itemId);

                if (warehouseItem == null)
                    return Result<Warehouse>.Failure("Item not found in warehouse", 404);

                _context.WarehouseItemEntities.Remove(warehouseItem);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                warehouseResult.Data.RemoveItemAsync(_mapper.Map<Item>(warehouseItem.Sku));
                return warehouseResult;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<Warehouse>.Failure(
                    $"Failed to remove item: {ex.Message}",
                    500,
                    ex.StackTrace);
            }
        }

        public async Task<Result<bool>> TransferItemAsync(
            Warehouse source,
            int itemId,
            int quantity,
            Warehouse target)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Verify source has enough stock
                var sourceItem = await _context.WarehouseItemEntities
                    .Include(wi => wi.Sku)
                    .FirstOrDefaultAsync(wi => wi.WarehouseId == source.WarehouseId && wi.Sku == itemId);

                if (sourceItem == null || sourceItem.ItemQuantity < quantity)
                    return Result<bool>.Failure("Insufficient stock for transfer", 400);

                // Update source
                sourceItem.ItemQuantity -= quantity;
                _context.WarehouseItemEntities.Update(sourceItem);

                // Update target
                var targetItem = await _context.WarehouseItemEntities
                    .FirstOrDefaultAsync(wi => wi.WarehouseId == target.WarehouseId && wi.Sku == itemId);

                if (targetItem == null)
                {
                    await _context.WarehouseItemEntities.AddAsync(new WarehouseItemEntity
                    {
                        WarehouseId = target.WarehouseId,
                        Sku = itemId,
                        ItemQuantity = quantity
                    });
                }
                else
                {
                    targetItem.ItemQuantity += quantity;
                    _context.WarehouseItemEntities.Update(targetItem);
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                // Update domain models
                await source.TransferItemAsync(itemId, quantity, target);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<bool>.Failure($"Transfer failed: {ex.Message}", 500);
            }
        }

        public async Task<Result<int>> GetAvailableStockAsync(int warehouseId, int itemId)
        {
            try
            {
                var item = await _context.WarehouseItemEntities
                    .FirstOrDefaultAsync(wi => wi.WarehouseId == warehouseId && wi.Sku == itemId);

                return Result<int>.Success(item?.ItemQuantity ?? 0);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(
                    $"Error checking stock: {ex.Message}",
                    500,
                    ex.StackTrace);
            }
        }

        public async Task<Result<bool>> SoftDeleteWarehouseAsync(int warehouseId)
        {
            try
            {
                var warehouse = await _context.WarehouseEntities
                    .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId);

                if (warehouse == null)
                {
                    return Result<bool>.Failure("Warehouse not found.");
                }

                warehouse.IsDeleted = true; // Mark as deleted
                _context.WarehouseEntities.Update(warehouse);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to delete warehouse: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UndeleteWarehouseAsync(int warehouseId)
        {
            try
            {
                var warehouse = await _context.WarehouseEntities
                    .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId);

                if (warehouse == null)
                {
                    return Result<bool>.Failure("Warehouse not found.");
                }

                if (!warehouse.IsDeleted)
                {
                    return Result<bool>.Failure("Warehouse is already active.");
                }

                warehouse.IsDeleted = false; // Restore the warehouse
                _context.WarehouseEntities.Update(warehouse);
                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to restore warehouse: {ex.Message}");
            }
        }


    }
}