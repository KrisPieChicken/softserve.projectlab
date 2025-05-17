using API.Data.Entities;
using softserve.projectlabs.Shared.Utilities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using API.Data;
using API.Models.Logistics.Order;

namespace API.Implementations.Domain
{
    public class OrderDomain
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ApplicationDbContext _context;
        private readonly WarehouseDomain _warehouseDomain;

        public OrderDomain(IOrderRepository orderRepository, ApplicationDbContext context, WarehouseDomain warehouseDomain)
        {
            _orderRepository = orderRepository;
            _context = context;
            _warehouseDomain = warehouseDomain;
        }

        public async Task<Result<Order>> GetOrderById(int orderId)
        {
            try
            {
                var orderEntity = await _orderRepository.GetByIdAsync(orderId);
                if (orderEntity == null)
                    return Result<Order>.Failure("Order not found.");

                var order = OrderMapper.ToDomain(orderEntity);
                return Result<Order>.Success(order);
            }
            catch (Exception ex)
            {
                return Result<Order>.Failure($"Failed to retrieve order: {ex.Message}");
            }
        }

        public async Task<Result<List<Order>>> GetAllOrders()
        {
            try
            {
                var orderEntities = await _context.OrderEntities
                    .Include(o => o.OrderItemEntities)
                    .ThenInclude(oi => oi.SkuNavigation)
                    .ToListAsync();

                var orders = orderEntities.Select(OrderMapper.ToDomain).ToList();
                return Result<List<Order>>.Success(orders);
            }
            catch (Exception ex)
            {
                return Result<List<Order>>.Failure($"Failed to retrieve orders: {ex.Message}");
            }
        }

        public async Task<Result<Order>> UpdateOrderFromCart(int orderId)
        {
            try
            {
                var orderEntity = await _context.OrderEntities
                    .Include(o => o.OrderItemEntities)
                    .FirstOrDefaultAsync(o => o.OrderId == orderId);

                if (orderEntity == null)
                    return Result<Order>.Failure("Order not found.");

                var cart = await _context.CartEntities
                    .Include(c => c.CartItemEntities)
                    .ThenInclude(ci => ci.SkuNavigation)
                    .FirstOrDefaultAsync(c => c.CustomerId == orderEntity.CustomerId);

                if (cart == null)
                    return Result<Order>.Failure("Associated cart not found.");

                orderEntity.OrderTotalAmount = cart.CartItemEntities.Sum(ci => ci.SkuNavigation.ItemPrice * ci.ItemQuantity);
                orderEntity.UpdatedAt = DateTime.UtcNow;

                foreach (var cartItem in cart.CartItemEntities)
                {
                    var existingOrderItem = orderEntity.OrderItemEntities
                        .FirstOrDefault(oi => oi.Sku == cartItem.SkuNavigation.ItemId);

                    if (existingOrderItem != null)
                    {
                        existingOrderItem.ItemQuantity = cartItem.ItemQuantity;
                    }
                    else
                    {
                        var newOrderItem = new OrderItemEntity
                        {
                            OrderId = orderEntity.OrderId,
                            Sku = cartItem.SkuNavigation.ItemId,
                            ItemQuantity = cartItem.ItemQuantity
                        };
                        orderEntity.OrderItemEntities.Add(newOrderItem);
                    }
                }

                _context.OrderEntities.Update(orderEntity);
                await _context.SaveChangesAsync();

                var updatedOrder = OrderMapper.ToDomain(orderEntity);
                return Result<Order>.Success(updatedOrder);
            }
            catch (Exception ex)
            {
                return Result<Order>.Failure($"Failed to update order from cart: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RetrieveAndSaveAllUnsavedOrders()
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    var carts = await _context.CartEntities
                        .Include(c => c.CartItemEntities)
                        .ThenInclude(ci => ci.SkuNavigation)
                        .Where(c => !_context.OrderEntities.Any(o => o.CustomerId == c.CustomerId))
                        .ToListAsync();

                    foreach (var cart in carts)
                    {
                        var orderEntity = new OrderEntity
                        {
                            CustomerId = cart.CustomerId,
                            OrderDate = DateTime.UtcNow,
                            OrderTotalAmount = cart.CartItemEntities.Sum(ci => ci.SkuNavigation.ItemPrice * ci.ItemQuantity),
                            OrderStatus = "Pending",
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        await _context.OrderEntities.AddAsync(orderEntity);
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    return Result<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    return Result<bool>.Failure($"Failed to retrieve and save unsaved orders: {ex.Message}");
                }
            });
        }

        public async Task<Result<bool>> DeleteOrder(int orderId)
        {
            try
            {
                await _orderRepository.DeleteAsync(orderId);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to delete order: {ex.Message}");
            }
        }

        public async Task<Result<Order>> RetrieveOrderByCartId(int cartId)
        {
            try
            {
                var cart = await _context.CartEntities
                    .Include(c => c.CartItemEntities)
                    .ThenInclude(ci => ci.SkuNavigation)
                    .FirstOrDefaultAsync(c => c.CartId == cartId);

                if (cart == null)
                    return Result<Order>.Failure("Cart not found.");

                var customerExists = await _context.CustomerEntities.AnyAsync(c => c.CustomerId == cart.CustomerId);
                if (!customerExists)
                    return Result<Order>.Failure($"Customer with ID {cart.CustomerId} does not exist.");

                var order = OrderMapper.FromCart(cart);
                return Result<Order>.Success(order);
            }
            catch (Exception ex)
            {
                return Result<Order>.Failure($"Failed to retrieve order: {ex.Message}");
            }
        }

        public async Task<Result<bool>> SaveUnsavedOrders()
        {
            try
            {
                var carts = await _context.CartEntities
                    .Include(c => c.CartItemEntities)
                    .ThenInclude(ci => ci.SkuNavigation)
                    .ToListAsync();

                foreach (var cart in carts)
                {
                    var existingOrder = await _context.OrderEntities
                        .FirstOrDefaultAsync(o => o.CustomerId == cart.CustomerId && !o.IsDeleted);

                    if (existingOrder == null)
                    {
                        var order = OrderMapper.FromCart(cart);
                        var orderEntity = OrderMapper.ToEntity(order);
                        await _context.OrderEntities.AddAsync(orderEntity);
                        await _context.SaveChangesAsync();
                    }
                }

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to save unsaved orders: {ex.Message}");
            }
        }

        public async Task<Result<bool>> FulfillOrder(int orderId )
        {
            try
            {
                var orderEntity = await _orderRepository.GetByIdAsync(orderId);
                if (orderEntity == null)
                    return Result<bool>.Failure("Order not found.");

                if (orderEntity.OrderStatus == "Fulfilled")
                    return Result<bool>.Failure("Order is already fulfilled.");

                orderEntity.OrderStatus = "Fulfilled";
                orderEntity.UpdatedAt = DateTime.UtcNow;

                await _orderRepository.UpdateAsync(orderEntity);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to fulfill order: {ex.Message}");
            }
        }
    }
}
