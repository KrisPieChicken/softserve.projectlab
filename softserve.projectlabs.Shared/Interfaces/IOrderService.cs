using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs.Order;

namespace softserve.projectlabs.Shared.Interfaces

{
    public interface IOrderService
    {
        Task<Result<OrderDto>> GetOrderByIdAsync(int orderId);
        Task<Result<List<OrderDto>>> GetAllOrdersAsync();
        Task<Result<OrderDto>> UpdateOrderAsync(OrderDto order);
        Task<Result<bool>> DeleteOrderAsync(int orderId);
        Task<Result<OrderDto>> RetrieveOrderByCartIdAsync(int cartId);
        Task<Result<bool>> FulfillOrderAsync(int orderId);
        Task<Result<bool>> RetrieveAndSaveAllUnsavedOrdersAsync();
    }
}
