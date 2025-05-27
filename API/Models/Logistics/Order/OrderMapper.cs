using API.Data.Entities;
using softserve.projectlabs.Shared.DTOs;
using softserve.projectlabs.Shared.DTOs.Order;

namespace API.Models.Logistics.Order
{
    public static class OrderMapper
    {
        public static Order ToDomain(OrderEntity entity)
            => new Order(
                entity.OrderId,
                entity.CustomerId,
                entity.OrderDate,
                entity.OrderStatus,
                entity.OrderItemEntities?.Select(oi =>
                    new OrderItem(oi.Sku, oi.SkuNavigation?.ItemName ?? "", oi.ItemQuantity, oi.SkuNavigation?.ItemPrice ?? 0)
                ).ToList() ?? new List<OrderItem>()
            );

        public static OrderEntity ToEntity(Order order)
        {
            return new OrderEntity
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                OrderStatus = order.OrderStatus,
                OrderTotalAmount = order.TotalAmount,
                OrderItemEntities = order.Items.Select(oi => new OrderItemEntity
                {
                    Sku = oi.Sku,
                    ItemQuantity = oi.Quantity
                }).ToList()
            };
        }

        public static OrderDto ToDto(Order order)
            => new OrderDto
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderStatus = order.OrderStatus,
                Items = order.Items.Select(oi => new OrderItemDto
                {
                    Sku = oi.Sku,
                    ItemName = oi.ItemName,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice
                }).ToList()
            };

        public static Order FromCart(CartEntity cart)
        {
            var items = cart.CartItemEntities.Select(ci =>
                new OrderItem(
                    ci.SkuNavigation.ItemId,
                    ci.SkuNavigation.ItemName,
                    ci.ItemQuantity,
                    ci.SkuNavigation.ItemPrice
                )).ToList();

            return new Order(
                0, // OrderId will be generated
                cart.CustomerId,
                DateTime.UtcNow,
                "Pending",
                items
            );
        }
    }

}
