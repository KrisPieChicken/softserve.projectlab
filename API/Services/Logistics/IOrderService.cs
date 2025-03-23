﻿using API.Models.Logistics;
using API.Models;

namespace API.Services.Logistics
{
    public interface IOrderService
    {
        Task<Result<Order>> CreateOrderAsync(Order order);
        Task<Result<Order>> GetOrderByIdAsync(int orderId);
        Task<Result<List<Order>>> GetAllOrdersAsync();
        Task<Result<Order>> UpdateOrderAsync(Order order);
        Task<Result<bool>> DeleteOrderAsync(int orderId);
    }
}
