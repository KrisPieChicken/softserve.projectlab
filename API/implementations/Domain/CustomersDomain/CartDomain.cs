using API.Data;
using API.Data.Entities;
using API.Models.Customers;
using API.Models.IntAdmin;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Implementations.Domain
{
    public class CartDomain
    {
        private readonly ApplicationDbContext _context;

        public CartDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Cart>> CreateCartAsync(int customerId)
        {
            try
            {
                // Verificar si el cliente existe
                var customerEntity = await _context.CustomerEntities.FindAsync(customerId);
                if (customerEntity == null)
                {
                    return Result<Cart>.Failure("Cliente no encontrado");
                }

                // Verificar si el cliente ya tiene un carrito activo
                var existingCart = await _context.CartEntities
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsDeleted);

                if (existingCart != null)
                {
                    return await GetCartByIdAsync(existingCart.CartId.ToString());
                }

                // Crear la entidad de carrito
                var cartEntity = new CartEntity
                {
                    CustomerId = customerId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    IsDeleted = false
                };

                _context.CartEntities.Add(cartEntity);
                await _context.SaveChangesAsync();

                // Crear el objeto de carrito a devolver
                var cart = new Cart
                {
                    Id = cartEntity.CartId.ToString(),
                    Customer = new Customer { CustomerId = customerId },
                    Items = new List<CartItem>(),
                    CreatedAt = cartEntity.CreatedAt,
                    UpdatedAt = cartEntity.UpdatedAt
                };

                return Result<Cart>.Success(cart);
            }
            catch (Exception ex)
            {
                return Result<Cart>.Failure($"Error al crear el carrito: {ex.Message}");
            }
        }

        public async Task<Result<Cart>> GetCartByIdAsync(string cartId)
        {
            try
            {
                if (!int.TryParse(cartId, out int id))
                {
                    return Result<Cart>.Failure("ID de carrito inválido");
                }

                var cartEntity = await _context.CartEntities
                    .Include(c => c.Customer)
                    .Include(c => c.CartItemEntities)
                        .ThenInclude(ci => ci.SkuNavigation)
                    .FirstOrDefaultAsync(c => c.CartId == id && !c.IsDeleted);

                if (cartEntity == null)
                {
                    return Result<Cart>.Failure("Carrito no encontrado");
                }

                var cart = MapToModel(cartEntity);
                return Result<Cart>.Success(cart);
            }
            catch (Exception ex)
            {
                return Result<Cart>.Failure($"Error al obtener el carrito: {ex.Message}");
            }
        }

        public async Task<Result<Cart>> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                var cartEntity = await _context.CartEntities
                    .Include(c => c.Customer)
                    .Include(c => c.CartItemEntities)
                        .ThenInclude(ci => ci.SkuNavigation)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsDeleted);

                if (cartEntity == null)
                {
                    return Result<Cart>.Failure("Carrito no encontrado para el cliente");
                }

                var cart = MapToModel(cartEntity);
                return Result<Cart>.Success(cart);
            }
            catch (Exception ex)
            {
                return Result<Cart>.Failure($"Error al obtener el carrito: {ex.Message}");
            }
        }

        public async Task<Result<Cart>> AddItemToCartAsync(string cartId, int itemSku, int quantity)
        {
            try
            {
                if (!int.TryParse(cartId, out int id))
                {
                    return Result<Cart>.Failure("ID de carrito inválido");
                }

                if (quantity <= 0)
                {
                    return Result<Cart>.Failure("La cantidad debe ser positiva");
                }

                // Verificar si el carrito existe
                var cartEntity = await _context.CartEntities
                    .Include(c => c.CartItemEntities)
                    .FirstOrDefaultAsync(c => c.CartId == id && !c.IsDeleted);

                if (cartEntity == null)
                {
                    return Result<Cart>.Failure("Carrito no encontrado");
                }

                // Verificar si el item existe
                var itemEntity = await _context.ItemEntities.FindAsync(itemSku);
                if (itemEntity == null)
                {
                    return Result<Cart>.Failure("Item no encontrado");
                }

                // Verificar si el item ya está en el carrito
                var existingItem = cartEntity.CartItemEntities
                    .FirstOrDefault(ci => ci.Sku == itemSku);

                if (existingItem != null)
                {
                    // Actualizar la cantidad
                    existingItem.ItemQuantity += quantity;
                }
                else
                {
                    // Añadir nuevo item al carrito
                    var cartItemEntity = new CartItemEntity
                    {
                        CartId = id,
                        Sku = itemSku,
                        ItemQuantity = quantity
                    };

                    _context.CartItemEntities.Add(cartItemEntity);
                }

                // Actualizar la fecha de actualización del carrito
                cartEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el carrito actualizado
                return await GetCartByIdAsync(cartId);
            }
            catch (Exception ex)
            {
                return Result<Cart>.Failure($"Error al añadir item al carrito: {ex.Message}");
            }
        }

        public async Task<Result<Cart>> RemoveItemFromCartAsync(string cartId, int itemSku, int quantity)
        {
            try
            {
                if (!int.TryParse(cartId, out int id))
                {
                    return Result<Cart>.Failure("ID de carrito inválido");
                }

                if (quantity <= 0)
                {
                    return Result<Cart>.Failure("La cantidad debe ser positiva");
                }

                // Verificar si el carrito existe
                var cartEntity = await _context.CartEntities
                    .Include(c => c.CartItemEntities)
                    .FirstOrDefaultAsync(c => c.CartId == id && !c.IsDeleted);

                if (cartEntity == null)
                {
                    return Result<Cart>.Failure("Carrito no encontrado");
                }

                // Verificar si el item existe en el carrito
                var existingItem = cartEntity.CartItemEntities
                    .FirstOrDefault(ci => ci.Sku == itemSku);

                if (existingItem == null)
                {
                    return Result<Cart>.Failure($"Item con SKU {itemSku} no encontrado en el carrito");
                }

                // Actualizar o eliminar el item del carrito
                if (existingItem.ItemQuantity <= quantity)
                {
                    _context.CartItemEntities.Remove(existingItem);
                }
                else
                {
                    existingItem.ItemQuantity -= quantity;
                }

                // Actualizar la fecha de actualización del carrito
                cartEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el carrito actualizado
                return await GetCartByIdAsync(cartId);
            }
            catch (Exception ex)
            {
                return Result<Cart>.Failure($"Error al eliminar item del carrito: {ex.Message}");
            }
        }

        public async Task<Result<Cart>> ClearCartAsync(string cartId)
        {
            try
            {
                if (!int.TryParse(cartId, out int id))
                {
                    return Result<Cart>.Failure("ID de carrito inválido");
                }

                // Verificar si el carrito existe
                var cartEntity = await _context.CartEntities
                    .Include(c => c.CartItemEntities)
                    .FirstOrDefaultAsync(c => c.CartId == id && !c.IsDeleted);

                if (cartEntity == null)
                {
                    return Result<Cart>.Failure("Carrito no encontrado");
                }

                // Eliminar todos los items del carrito
                _context.CartItemEntities.RemoveRange(cartEntity.CartItemEntities);

                // Actualizar la fecha de actualización del carrito
                cartEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                // Obtener el carrito actualizado
                return await GetCartByIdAsync(cartId);
            }
            catch (Exception ex)
            {
                return Result<Cart>.Failure($"Error al vaciar el carrito: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteCartAsync(string cartId)
        {
            try
            {
                if (!int.TryParse(cartId, out int id))
                {
                    return Result<bool>.Failure("ID de carrito inválido");
                }

                var cartEntity = await _context.CartEntities.FindAsync(id);
                if (cartEntity == null)
                {
                    return Result<bool>.Failure("Carrito no encontrado");
                }

                // Marcar como eliminado en lugar de eliminar físicamente
                cartEntity.IsDeleted = true;
                cartEntity.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error al eliminar el carrito: {ex.Message}");
            }
        }

        public async Task<Result<decimal>> GetCartTotalAsync(string cartId)
        {
            try
            {
                if (!int.TryParse(cartId, out int id))
                {
                    return Result<decimal>.Failure("ID de carrito inválido");
                }

                var cartEntity = await _context.CartEntities
                    .Include(c => c.CartItemEntities)
                        .ThenInclude(ci => ci.SkuNavigation)
                    .FirstOrDefaultAsync(c => c.CartId == id && !c.IsDeleted);

                if (cartEntity == null)
                {
                    return Result<decimal>.Failure("Carrito no encontrado");
                }

                decimal total = 0;
                foreach (var item in cartEntity.CartItemEntities)
                {
                    total += item.SkuNavigation.ItemUnitCost * item.ItemQuantity;
                }

                return Result<decimal>.Success(total);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure($"Error al calcular el total del carrito: {ex.Message}");
            }
        }

        private Cart MapToModel(CartEntity entity)
        {
            var cart = new Cart
            {
                Id = entity.CartId.ToString(),
                Customer = new Customer
                {
                    CustomerId = entity.CustomerId,
                    CustomerFirstName = entity.Customer.CustomerFirstName ?? string.Empty,
                    CustomerLastName = entity.Customer.CustomerLastName ?? string.Empty,
                    CustomerContactEmail = entity.Customer.CustomerContactEmail ?? string.Empty,
                    CustomerContactNumber = entity.Customer.CustomerContactNumber ?? string.Empty
                },
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt,
                Items = new List<CartItem>()
            };

            // Mapear los items del carrito
            if (entity.CartItemEntities != null)
            {
                foreach (var itemEntity in entity.CartItemEntities)
                {
                    var item = new Item
                    {
                        Sku = itemEntity.Sku,
                        ItemName = itemEntity.SkuNavigation.ItemName,
                        ItemDescription = itemEntity.SkuNavigation.ItemDescription,
                        ItemUnitCost = itemEntity.SkuNavigation.ItemUnitCost,
                        ItemPrice = itemEntity.SkuNavigation.ItemPrice
                    };

                    cart.Items.Add(new CartItem
                    {
                        Item = item,
                        Quantity = itemEntity.ItemQuantity
                    });
                }
            }

            return cart;
        }
    }
}