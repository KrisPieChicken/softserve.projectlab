using API.Models.Customers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers.Customers
{
    /// <summary>
    /// API Controller for managing Cart operations.
    /// </summary>
    [ApiController]
    [Route("api/carts")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        /// <summary>
        /// Constructor with dependency injection for ICartService.
        /// </summary>
        /// <param name="cartService">The cart service instance</param>
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Creates a new cart for a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer to create the cart for</param>
        /// <returns>HTTP response with the created cart or error message</returns>
        [HttpPost("customer/{customerId}")]
        public async Task<IActionResult> CreateCart(int customerId)
        {
            var result = await _cartService.CreateCartAsync(customerId);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a cart by its unique ID.
        /// </summary>
        /// <param name="id">Unique identifier of the cart</param>
        /// <returns>HTTP response with the cart or error message</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(string id)
        {
            var result = await _cartService.GetCartByIdAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a customer's cart by the customer ID.
        /// </summary>
        /// <param name="customerId">The ID of the customer whose cart is to be retrieved</param>
        /// <returns>HTTP response with the customer's cart or error message</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCartByCustomerId(int customerId)
        {
            var result = await _cartService.GetCartByCustomerIdAsync(customerId);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Adds an item to a cart.
        /// </summary>
        /// <param name="id">The ID of the cart</param>
        /// <param name="itemSku">The SKU of the item to add</param>
        /// <param name="quantity">The quantity to add (defaults to 1)</param>
        /// <returns>HTTP response with the updated cart or error message</returns>
        [HttpPost("{id}/items/{itemSku}")]
        public async Task<IActionResult> AddItemToCart(string id, int itemSku, [FromQuery] int quantity = 1)
        {
            var result = await _cartService.AddItemToCartAsync(id, itemSku, quantity);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes an item from a cart.
        /// </summary>
        /// <param name="id">The ID of the cart</param>
        /// <param name="itemSku">The SKU of the item to remove</param>
        /// <param name="quantity">The quantity to remove (defaults to 1)</param>
        /// <returns>HTTP response with the updated cart or error message</returns>
        [HttpDelete("{id}/items/{itemSku}")]
        public async Task<IActionResult> RemoveItemFromCart(string id, int itemSku, [FromQuery] int quantity = 1)
        {
            var result = await _cartService.RemoveItemFromCartAsync(id, itemSku, quantity);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Clears all items from a cart.
        /// </summary>
        /// <param name="id">The ID of the cart to clear</param>
        /// <returns>HTTP response with the empty cart or error message</returns>
        [HttpDelete("{id}/clear")]
        public async Task<IActionResult> ClearCart(string id)
        {
            var result = await _cartService.ClearCartAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a cart by its unique ID.
        /// </summary>
        /// <param name="id">Unique identifier of the cart to remove</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(string id)
        {
            var result = await _cartService.DeleteCartAsync(id);
            return result.IsSuccess
                ? NoContent()
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Calculates the total price of all items in a cart.
        /// </summary>
        /// <param name="id">The ID of the cart</param>
        /// <returns>HTTP response with the cart total or error message</returns>
        [HttpGet("{id}/total")]
        public async Task<IActionResult> GetCartTotal(string id)
        {
            var result = await _cartService.GetCartTotalAsync(id);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }
    }
}