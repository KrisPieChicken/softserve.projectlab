using API.Implementations.Domain;
using API.Models.Logistics.Order;
using softserve.projectlabs.Shared.DTOs.Order;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly OrderDomain _orderDomain;

        public OrderService(OrderDomain orderDomain)
        {
            _orderDomain = orderDomain;
        }

        public async Task<Result<OrderDto>> GetOrderByIdAsync(int orderId)
        {
            var result = await _orderDomain.GetOrderById(orderId);
            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.ErrorMessage);

            var orderDto = OrderMapper.ToDto(result.Data);
            return Result<OrderDto>.Success(orderDto);
        }

        public async Task<Result<OrderDto>> RetrieveOrderByCartIdAsync(int cartId)
        {
            var result = await _orderDomain.RetrieveOrderByCartId(cartId);
            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.ErrorMessage);

            var orderDto = OrderMapper.ToDto(result.Data);
            return Result<OrderDto>.Success(orderDto);
        }

        public async Task<Result<List<OrderDto>>> GetAllOrdersAsync()
        {
            var result = await _orderDomain.GetAllOrders();
            if (!result.IsSuccess)
                return Result<List<OrderDto>>.Failure(result.ErrorMessage);

            var orderDtos = result.Data.Select(OrderMapper.ToDto).ToList();
            return Result<List<OrderDto>>.Success(orderDtos);
        }

        public async Task<Result<bool>> RetrieveAndSaveAllUnsavedOrdersAsync()
        {
            return await _orderDomain.RetrieveAndSaveAllUnsavedOrders();
        }

        public async Task<Result<OrderDto>> UpdateOrderAsync(OrderDto orderDto)
        {
            var result = await _orderDomain.UpdateOrderFromCart(orderDto.OrderId);
            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.ErrorMessage);

            var updatedOrderDto = OrderMapper.ToDto(result.Data);
            return Result<OrderDto>.Success(updatedOrderDto);
        }

        public async Task<Result<bool>> DeleteOrderAsync(int orderId)
        {
            return await _orderDomain.DeleteOrder(orderId);
        }

        public async Task<Result<bool>> SaveUnsavedOrdersAsync()
        {
            return await _orderDomain.SaveUnsavedOrders();
        }

        public async Task<Result<bool>> FulfillOrderAsync(int orderId)
        {
            // Optionally, get the current employee/user from context if needed If needed and depnded on the business logic
            // string fulfilledBy = ...;

            var result = await _orderDomain.FulfillOrder(orderId /*, fulfilledBy */);

            return result;
        }
    }
}
