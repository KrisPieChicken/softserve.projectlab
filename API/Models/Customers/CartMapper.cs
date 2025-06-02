using softserve.projectlabs.Shared.DTOs.Cart;

namespace API.Models.Customers
{
    public static class CartMapper
    {
        public static CartDto ToDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                CustomerId = cart.Customer?.CustomerId ?? 0,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                Items = cart.Items?.Select(ci => new CartItemDto
                {
                    CartId = cart.Id,
                    ItemSku = ci.Item?.Sku ?? 0,
                    Quantity = ci.Quantity
                }).ToList() ?? new List<CartItemDto>()
            };
        }
    }
}
