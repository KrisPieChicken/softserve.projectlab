using softserve.projectlabs.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace softserve.projectlabs.Shared.Interfaces
{
    public interface ISupplierOrderService
    {
        Task<List<SupplierOrderDto>> GetAllSupplierOrdersAsync();
        Task<SupplierOrderDto> GetSupplierOrderByIdAsync(int orderId);
        Task<SupplierOrderDto> AddSupplierOrderAsync(SupplierOrderDto order);
        Task<bool> UpdateSupplierOrderAsync(SupplierOrderDto order);
        Task<bool> DeleteSupplierOrderAsync(int orderId);
    }
}
