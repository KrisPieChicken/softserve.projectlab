using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs;
using softserve.projectlabs.Shared.Utilities;

namespace softserve.projectlabs.Shared.Interfaces
{
    public interface IWarehouseService
    {
        Task<List<WarehouseResponseDto>> GetWarehousesAsync();
        Task<Result<WarehouseResponseDto>> GetWarehouseByIdAsync(int warehouseId);
        Task<Result<bool>> AddItemToWarehouseAsync(int warehouseId, AddItemToWarehouseDTO itemDto);
        Task<Result<bool>> RemoveItemFromWarehouseAsync(int warehouseId, int itemId);
        Task<Result<int>> CheckWarehouseStockAsync(int warehouseId, int sku);
        Task<Result<bool>> TransferItemAsync(int sourceWarehouseId, int sku, int quantity, int targetWarehouseId);
        Task<Result<List<ItemDto>>> GetLowStockItemsAsync(int warehouseId, int threshold);
        Task<Result<decimal>> CalculateTotalInventoryValueAsync(int warehouseId);
        Task<Result<string>> GenerateInventoryReportAsync(int warehouseId);
        Task<Result<bool>> DeleteWarehouseAsync(int warehouseId);
        Task<Result<bool>> UndeleteWarehouseAsync(int warehouseId);
    }
}
