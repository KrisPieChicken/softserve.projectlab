using API.Implementations.Domain;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.DTOs;
using softserve.projectlabs.Shared.DTOs.Item;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Services.Logistics
{
    public class WarehouseService : IWarehouseService
    {
        private readonly WarehouseDomain _warehouseDomain;

        public WarehouseService(WarehouseDomain warehouseDomain)
        {
            _warehouseDomain = warehouseDomain;
        }

        public async Task<List<WarehouseResponseDto>> GetAllWarehousesAsync()
        {
            var result = await _warehouseDomain.GetAllWarehousesAsync();
            if (!result.IsSuccess)
                return new List<WarehouseResponseDto>();

            return result.Data.Select(warehouse =>
            {
                return new WarehouseResponseDto
                {
                    WarehouseId = warehouse.WarehouseId,
                    Name = warehouse.Name,
                    Location = warehouse.Location,
                    Capacity = warehouse.Capacity,
                    BranchId = warehouse.BranchId,
                    Items = warehouse.Items.Select(item => new ItemDto
                    {
                        ItemId = item.ItemId,
                        Sku = item.Sku,
                        ItemName = item.ItemName,
                        ItemDescription = item.ItemDescription,
                        CurrentStock = item.CurrentStock
                    }).ToList()
                };
            }).ToList();
        }

        public async Task<Result<WarehouseResponseDto>> GetWarehouseByIdAsync(int warehouseId)
        {
            var result = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
            if (!result.IsSuccess)
                return Result<WarehouseResponseDto>.Failure(result.ErrorMessage, result.ErrorCode);

            var warehouse = result.Data;
            var warehouseDto = new WarehouseResponseDto
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                Location = warehouse.Location,
                Capacity = warehouse.Capacity,
                BranchId = warehouse.BranchId,
                Items = warehouse.Items.Select(item => new ItemDto
                {
                    ItemId = item.ItemId,
                    Sku = item.Sku,
                    ItemName = item.ItemName,
                    ItemDescription = item.ItemDescription,
                    CurrentStock = item.CurrentStock
                }).ToList()
            };

            return Result<WarehouseResponseDto>.Success(warehouseDto);
        }

        public async Task<Result<bool>> AddItemToWarehouseAsync(int warehouseId, int sku, int quantity)
        {
            var warehouseResult = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
            if (!warehouseResult.IsSuccess)
                return Result<bool>.Failure(warehouseResult.ErrorMessage);

            var warehouse = warehouseResult.Data;
            warehouse.AddItem(new Item { Sku = sku, CurrentStock = quantity });

            return await _warehouseDomain.UpdateWarehouseAsync(warehouse);
        }

        public async Task<Result<bool>> RemoveItemFromWarehouseAsync(int warehouseId, int sku)
        {
            var warehouseResult = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
            if (!warehouseResult.IsSuccess)
                return Result<bool>.Failure(warehouseResult.ErrorMessage);

            var warehouse = warehouseResult.Data;
            warehouse.RemoveItem(sku);

            return await _warehouseDomain.UpdateWarehouseAsync(warehouse);
        }

        public async Task<Result<bool>> TransferItemAsync(int sourceWarehouseId, int sku, int quantity, int targetWarehouseId)
        {
            var sourceResult = await _warehouseDomain.GetWarehouseByIdAsync(sourceWarehouseId);
            var targetResult = await _warehouseDomain.GetWarehouseByIdAsync(targetWarehouseId);

            if (!sourceResult.IsSuccess || !targetResult.IsSuccess)
                return Result<bool>.Failure("One or both warehouses not found.");

            var sourceWarehouse = sourceResult.Data;
            var targetWarehouse = targetResult.Data;

            sourceWarehouse.TransferItem(sku, quantity, targetWarehouse);

            await _warehouseDomain.UpdateWarehouseAsync(sourceWarehouse);
            await _warehouseDomain.UpdateWarehouseAsync(targetWarehouse);

            return Result<bool>.Success(true);
        }

        public async Task<Result<List<ItemDto>>> GetLowStockItemsAsync(int warehouseId, int threshold)
        {
            var result = await _warehouseDomain.GetLowStockItemsAsync(warehouseId, threshold);
            if (!result.IsSuccess)
                return Result<List<ItemDto>>.Failure(result.ErrorMessage);

            return Result<List<ItemDto>>.Success(result.Data.Select(item => new ItemDto
            {
                ItemId = item.ItemId,
                Sku = item.Sku,
                ItemName = item.ItemName,
                ItemDescription = item.ItemDescription,
                CurrentStock = item.CurrentStock
            }).ToList());
        }

        public async Task<Result<decimal>> CalculateTotalInventoryValueAsync(int warehouseId)
        {
            return await _warehouseDomain.GetTotalInventoryValueAsync(warehouseId);
        }

        public async Task<Result<string>> GenerateInventoryReportAsync(int warehouseId)
        {
            return await _warehouseDomain.GenerateInventoryReportAsync(warehouseId);
        }

        public async Task<Result<bool>> SoftDeleteWarehouseAsync(int warehouseId)
        {
            return await _warehouseDomain.SoftDeleteWarehouseAsync(warehouseId);
        }

        public async Task<Result<bool>> UndeleteWarehouseAsync(int warehouseId)
        {
            return await _warehouseDomain.UndeleteWarehouseAsync(warehouseId);
        }

        public async Task<Result<bool>> DeleteWarehouseAsync(int warehouseId)
        {
            return await _warehouseDomain.SoftDeleteWarehouseAsync(warehouseId);
        }

        public async Task<Result<WarehouseResponseDto>> CreateWarehouseAsync(WarehouseDto warehouseDto)
        {
            var result = await _warehouseDomain.CreateWarehouseAsync(warehouseDto);
            if (!result.IsSuccess)
                return Result<WarehouseResponseDto>.Failure(result.ErrorMessage);

            var warehouse = result.Data;
            var warehouseResponse = new WarehouseResponseDto
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                Location = warehouse.Location,
                Capacity = warehouse.Capacity,
                BranchId = warehouse.BranchId
            };

            return Result<WarehouseResponseDto>.Success(warehouseResponse);
        }

        public async Task<Result<int>> CheckWarehouseStockAsync(int warehouseId, int sku)
        {
            var warehouseResult = await _warehouseDomain.GetWarehouseByIdAsync(warehouseId);
            if (!warehouseResult.IsSuccess)
                return Result<int>.Failure(warehouseResult.ErrorMessage);

            var item = warehouseResult.Data.Items.FirstOrDefault(i => i.Sku == sku);
            if (item == null)
                return Result<int>.Failure("Item not found in warehouse.");

            return Result<int>.Success(item.CurrentStock);
        }
    }
}
