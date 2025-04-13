using API.Data.Entities;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.Logistics.Interfaces
{
    public interface ISupplier
    {
        int SupplierId { get; set; }
        string SupplierName { get; set; }
        string SupplierAddress { get; set; }
        string SupplierContactNumber { get; set; }
        string SupplierContactEmail { get; set; }
        List<Item> ProductsSupplied { get; set; }
        bool IsActive { get; set; }
        List<SupplierOrder> Orders { get; set; }

        Result<ISupplier> AddSupplier(ISupplier supplier);
        Result<ISupplier> UpdateSupplier(ISupplier supplier);
        Result<ISupplier> GetSupplierById(int supplierId);
        Result<List<ISupplier>> GetAllSuppliers();
        Result<bool> DeleteSupplier(int supplierId);
    }
}
