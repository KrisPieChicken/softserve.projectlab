using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs.Item;
using softserve.projectlabs.Shared.DTOs.Warehouse;
using softserve.projectlabs.Shared.Utilities;

namespace softserve.projectlabs.Shared.Interfaces
{
    public interface IWarehouseService
    {
        Task<List<WarehouseResponseDto>> GetAllWarehousesAsync();
        Task<Result<WarehouseResponseDto>> GetWarehouseByIdAsync(int warehouseId);
        Task<Result<bool>> AddItemToWarehouseAsync(int warehouseId, int sku, int quantity);
        Task<Result<bool>> RemoveItemFromWarehouseAsync(int warehouseId, int itemId);
        Task<Result<int>> CheckWarehouseStockAsync(int warehouseId, int sku);
        Task<Result<bool>> TransferItemAsync(int sourceWarehouseId, int sku, int quantity, int targetWarehouseId);
        Task<Result<List<ItemDto>>> GetLowStockItemsAsync(int warehouseId, int threshold);
        Task<Result<decimal>> CalculateTotalInventoryValueAsync(int warehouseId);
        Task<Result<string>> GenerateInventoryReportAsync(int warehouseId);
        Task<Result<bool>> DeleteWarehouseAsync(int warehouseId);
        Task<Result<bool>> UndeleteWarehouseAsync(int warehouseId);
        Task<Result<WarehouseResponseDto>> CreateWarehouseAsync(WarehouseDto warehouseDto);
        Task<Result<bool>> SoftDeleteWarehouseAsync(int warehouseId);



    }

}
