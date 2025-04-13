using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.Logistics.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Domain.Logistics
{
    public class SupplierOrderDomain
    {
        private readonly List<ISupplierOrder> _orders = new(); // Mock Data

        public Task<List<ISupplierOrder>> GetAllSupplierOrdersAsync()
        {
            return Task.FromResult(_orders);
        }

        public Task<ISupplierOrder> GetSupplierOrderByIdAsync(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            return Task.FromResult(order);
        }

        public Task<ISupplierOrder> AddSupplierOrderAsync(ISupplierOrder order)
        {
            _orders.Add(order);
            return Task.FromResult(order);
        }

        public Task<bool> UpdateSupplierOrderAsync(ISupplierOrder order)
        {
            var existing = _orders.FirstOrDefault(o => o.OrderId == order.OrderId);
            if (existing == null) return Task.FromResult(false);

            _orders.Remove(existing);
            _orders.Add(order);
            return Task.FromResult(true);
        }

        public Task<bool> DeleteSupplierOrderAsync(int orderId)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null) return Task.FromResult(false);

            _orders.Remove(order);
            return Task.FromResult(true);
        }
    }
}
