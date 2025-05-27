using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs.Supplier;

namespace softserve.projectlabs.Shared.Interfaces

{
    public interface ISupplierService
    {
        Task<Result<SupplierDto>> CreateSupplierAsync(SupplierDto supplier);
        Task<Result<SupplierDto>> GetSupplierByIdAsync(int supplierId);
        Task<Result<List<SupplierDto>>> GetAllSuppliersAsync();
        Task<Result<SupplierDto>> UpdateSupplierAsync(SupplierDto supplier);
        Task<Result<bool>> DeleteSupplierAsync(int supplierId);
        Task<Result<bool>> AddItemToSupplierAsync(int supplierId, int sku);
        Task<Result<bool>> UndeleteSupplierAsync(int supplierId);



    }
}
