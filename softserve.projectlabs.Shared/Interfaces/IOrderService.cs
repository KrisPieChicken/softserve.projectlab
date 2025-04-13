using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.DTOs;

namespace softserve.projectlabs.Shared.Interfaces

{
    public interface IOrderService
    {
        Task<Result<OrderDto>> CreateOrderAsync(OrderItemRequestDto orderItemRequest);
        Task<Result<OrderDto>> GetOrderByIdAsync(int orderId);
        Task<Result<List<OrderDto>>> GetAllOrdersAsync();
        Task<Result<OrderDto>> UpdateOrderAsync(OrderDto order);
        Task<Result<bool>> DeleteOrderAsync(int orderId);
    }
}
