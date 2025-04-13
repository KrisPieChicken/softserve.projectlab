using API.Implementations.Domain;
using API.Models.Logistics;
using API.Data.Entities;
using API.Models;
using API.Data;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;

namespace API.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly OrderDomain _orderDomain;

        public OrderService(ApplicationDbContext context, IMapper mapper, OrderDomain orderDomain)
        {
            _context = context;
            _mapper = mapper;
            _orderDomain = orderDomain;
        }

        public async Task<Result<OrderDto>> CreateOrderAsync(OrderItemRequestDto orderRequestDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var orderEntity = _mapper.Map<OrderEntity>(orderRequestDto);
                decimal totalAmount = 0;
                var itemsToUpdate = new List<ItemEntity>();

                foreach (var itemRequest in orderRequestDto.Items)
                {
                    var itemEntity = await _context.ItemEntities
                        .FirstOrDefaultAsync(i => i.Sku.ToString() == itemRequest.ItemId.ToString());

                    if (itemEntity == null)
                        return Result<OrderDto>.Failure($"Item SKU {itemRequest.ItemId} not found");

                    if (itemEntity.CurrentStock < itemRequest.Quantity)
                        return Result<OrderDto>.Failure($"Insufficient stock for SKU {itemRequest.ItemId}");

                    itemEntity.CurrentStock -= itemRequest.Quantity;
                    itemsToUpdate.Add(itemEntity);
                    totalAmount += itemEntity.ItemPrice * itemRequest.Quantity;
                }

                orderEntity.OrderTotalAmount = totalAmount;
                orderEntity.OrderDate = DateTime.UtcNow;
                orderEntity.OrderStatus = "Pending";

                _context.OrderEntities.Add(orderEntity);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Result<OrderDto>.Success(_mapper.Map<OrderDto>(orderEntity));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return Result<OrderDto>.Failure($"Order creation failed: {ex.Message}");
            }
        }

        public async Task<Result<OrderDto>> GetOrderByIdAsync(int orderId)
        {
            var result = await _orderDomain.GetOrderById(orderId);
            return result.IsSuccess
                ? Result<OrderDto>.Success(_mapper.Map<OrderDto>(result.Data))
                : Result<OrderDto>.Failure(result.ErrorMessage);
        }

        public async Task<Result<List<OrderDto>>> GetAllOrdersAsync()
        {
            var result = await _orderDomain.GetAllOrders();
            return result.IsSuccess
                ? Result<List<OrderDto>>.Success(_mapper.Map<List<OrderDto>>(result.Data))
                : Result<List<OrderDto>>.Failure(result.ErrorMessage);
        }

        public async Task<Result<OrderDto>> UpdateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<Order>(orderDto);
            var result = await _orderDomain.UpdateOrder(order);
            return result.IsSuccess
                ? Result<OrderDto>.Success(_mapper.Map<OrderDto>(result.Data))
                : Result<OrderDto>.Failure(result.ErrorMessage);
        }

        public async Task<Result<bool>> DeleteOrderAsync(int orderId)
        {
            return await _orderDomain.DeleteOrder(orderId);
        }
    }
}

