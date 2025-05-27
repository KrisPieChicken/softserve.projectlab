using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using softserve.projectlabs.Shared.Utilities;
using API.Models.IntAdmin;
using API.Models.Logistics.Warehouses;
using softserve.projectlabs.Shared.DTOs.Warehouse;

namespace API.Implementations.Domain
{
    public class WarehouseDomain
    {
        private readonly IWarehouseRepository _warehouseRepository;

        public WarehouseDomain(IWarehouseRepository warehouseRepository)
        {
            _warehouseRepository = warehouseRepository;
        }

        // Map Domain Model → Data Model
        public WarehouseEntity MapToEntity(Warehouse warehouse)
        {
            return new WarehouseEntity
            {
                WarehouseId = warehouse.WarehouseId,
                WarehouseLocation = warehouse.Location,
                WarehouseCapacity = warehouse.Capacity,
                BranchId = warehouse.BranchId,
                IsDeleted = false,
                WarehouseItemEntities = warehouse.Items.Select(item => new WarehouseItemEntity
                {
                    Sku = item.Sku,
                    ItemQuantity = item.CurrentStock
                }).ToList()
            };
        }

        // Map Data Model → Domain Model
        public Warehouse MapToDomainModel(WarehouseEntity entity)
        {
            var warehouse = new Warehouse(
                entity.WarehouseId,
                $"Warehouse {entity.WarehouseId}",
                entity.WarehouseLocation,
                entity.WarehouseCapacity,
                entity.BranchId,
                entity.WarehouseItemEntities?.Select(wi => new Item
                {
                    ItemId = wi.Sku,
                    Sku = wi.Sku,
                    ItemName = wi.SkuNavigation?.ItemName ?? "",
                    ItemDescription = wi.SkuNavigation?.ItemDescription ?? "",
                    CurrentStock = wi.ItemQuantity,
                    ItemPrice = wi.SkuNavigation?.ItemPrice ?? 0,
                }).ToList() ?? new List<Item>()
            );
            return warehouse;
        }

        public async Task<Result<Warehouse>> GetWarehouseByIdAsync(int warehouseId)
        {
            try
            {
                var entity = await _warehouseRepository.GetByIdAsync(warehouseId);
                if (entity == null)
                    return Result<Warehouse>.Failure("Warehouse not found.");

                var warehouse = MapToDomainModel(entity);
                return Result<Warehouse>.Success(warehouse);
            }
            catch (Exception ex)
            {
                return Result<Warehouse>.Failure($"Failed to retrieve warehouse: {ex.Message}");
            }
        }

        public async Task<Result<List<Warehouse>>> GetAllWarehousesAsync()
        {
            try
            {
                var entities = await _warehouseRepository.GetAllAsync();
                var warehouses = entities.Select(MapToDomainModel).ToList();
                return Result<List<Warehouse>>.Success(warehouses);
            }
            catch (Exception ex)
            {
                return Result<List<Warehouse>>.Failure($"Failed to retrieve warehouses: {ex.Message}");
            }
        }

        public async Task<Result<Warehouse>> CreateWarehouseAsync(WarehouseDto warehouseDto)
        {
            try
            {
                var warehouseEntity = new WarehouseEntity
                {
                    WarehouseLocation = warehouseDto.Location,
                    WarehouseCapacity = warehouseDto.Capacity,
                    BranchId = warehouseDto.BranchId,
                    IsDeleted = false
                };

                await _warehouseRepository.AddAsync(warehouseEntity);

                var warehouse = MapToDomainModel(warehouseEntity);
                return Result<Warehouse>.Success(warehouse);
            }
            catch (Exception ex)
            {
                return Result<Warehouse>.Failure($"Failed to create warehouse: {ex.Message}");
            }
        }

        public async Task<Result<bool>> UpdateWarehouseAsync(Warehouse warehouse)
        {
            try
            {
                var warehouseEntity = await _warehouseRepository.GetByIdAsync(warehouse.WarehouseId);
                if (warehouseEntity == null)
                    return Result<bool>.Failure("Warehouse not found.");

                warehouseEntity.WarehouseLocation = warehouse.Location;
                warehouseEntity.WarehouseCapacity = warehouse.Capacity;
                warehouseEntity.BranchId = warehouse.BranchId;

                warehouseEntity.WarehouseItemEntities = warehouse.Items
                    .Select(item => new WarehouseItemEntity
                    {
                        WarehouseId = warehouse.WarehouseId,
                        Sku = item.Sku,
                        ItemQuantity = item.CurrentStock
                    }).ToList();

                await _warehouseRepository.UpdateAsync(warehouseEntity);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to update warehouse: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SoftDeleteWarehouseAsync(int warehouseId)
        {
            try
            {
                var warehouseEntity = await _warehouseRepository.GetByIdAsync(warehouseId);
                if (warehouseEntity == null)
                    return Result<bool>.Failure("Warehouse not found.");

                warehouseEntity.IsDeleted = true;
                await _warehouseRepository.UpdateAsync(warehouseEntity);
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
                var warehouseEntity = await _warehouseRepository.GetByIdAsync(warehouseId);
                if (warehouseEntity == null)
                    return Result<bool>.Failure("Warehouse not found.");

                warehouseEntity.IsDeleted = false;
                await _warehouseRepository.UpdateAsync(warehouseEntity);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to restore warehouse: {ex.Message}");
            }
        }

        public async Task<Result<List<Item>>> GetLowStockItemsAsync(int warehouseId, int threshold)
        {
            var warehouseResult = await GetWarehouseByIdAsync(warehouseId);
            if (!warehouseResult.IsSuccess)
                return Result<List<Item>>.Failure(warehouseResult.ErrorMessage);

            var lowStockItems = warehouseResult.Data.Items.Where(item => item.CurrentStock <= threshold).ToList();
            return Result<List<Item>>.Success(lowStockItems);
        }

        public async Task<Result<decimal>> GetTotalInventoryValueAsync(int warehouseId)
        {
            var warehouseResult = await GetWarehouseByIdAsync(warehouseId);
            if (!warehouseResult.IsSuccess)
                return Result<decimal>.Failure(warehouseResult.ErrorMessage);

            var totalValue = warehouseResult.Data.Items.Sum(item => item.CurrentStock * item.ItemPrice);
            return Result<decimal>.Success(totalValue);
        }

        public async Task<Result<string>> GenerateInventoryReportAsync(int warehouseId)
        {
            var warehouseResult = await GetWarehouseByIdAsync(warehouseId);
            if (!warehouseResult.IsSuccess)
                return Result<string>.Failure(warehouseResult.ErrorMessage);

            var report = System.Text.Json.JsonSerializer.Serialize(warehouseResult.Data.Items);
            return Result<string>.Success(report);
        }

        public async Task<Result<bool>> ReserveStockForOrderAsync(int warehouseId, int sku, int quantity)
        {
            try
            {
                var warehouseEntity = await _warehouseRepository.GetByIdAsync(warehouseId);
                if (warehouseEntity == null)
                    return Result<bool>.Failure("Warehouse not found.");

                var warehouse = MapToDomainModel(warehouseEntity);
                var item = warehouse.Items.FirstOrDefault(i => i.Sku == sku);

                if (item == null || item.CurrentStock < quantity)
                    return Result<bool>.Failure("Insufficient stock for reservation.");

                item.CurrentStock -= quantity;

                await _warehouseRepository.UpdateAsync(MapToEntity(warehouse));
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to reserve stock: {ex.Message}");
            }
        }
    }
}
