using API.Models.Customers;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.DTOs.Cart;
using System.Threading.Tasks;

namespace API.Controllers.Customers
{
    /// <summary>
    /// Controller for managing customer shopping carts.
    /// </summary>
    [ApiController]
    [Route("api/carts")]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CartController"/> class.
        /// </summary>
        /// <param name="cartService">The cart service.</param>
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        /// <summary>
        /// Creates a new cart for a customer.
        /// </summary>
        /// <param name="customerId">The ID of the customer.</param>
        /// <returns>The created cart DTO or a bad request result.</returns>
        [HttpPost("customer/{customerId}")]
        public async Task<IActionResult> CreateCart(int customerId)
        {
            var result = await _cartService.CreateCartAsync(customerId);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            var dto = CartMapper.ToDto(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Gets a cart by its ID.
        /// </summary>
        /// <param name="id">The cart ID.</param>
        /// <returns>The cart DTO or a not found result.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCartById(string id)
        {
            var result = await _cartService.GetCartByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            var dto = CartMapper.ToDto(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Gets a cart by the customer ID.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>The cart DTO or a not found result.</returns>
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetCartByCustomerId(int customerId)
        {
            var result = await _cartService.GetCartByCustomerIdAsync(customerId);
            if (!result.IsSuccess)
                return NotFound(result.ErrorMessage);

            var dto = CartMapper.ToDto(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Adds an item to the cart or increases its quantity.
        /// </summary>
        /// <param name="id">The cart ID.</param>
        /// <param name="itemSku">The SKU of the item to add.</param>
        /// <param name="quantity">The quantity to add (default is 1).</param>
        /// <returns>The updated cart DTO or a bad request result.</returns>
        [HttpPost("{id}/items/{itemSku}")]
        public async Task<IActionResult> AddItemToCart(string id, int itemSku, [FromQuery] int quantity = 1)
        {
            var result = await _cartService.AddItemToCartAsync(id, itemSku, quantity);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            var dto = CartMapper.ToDto(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Removes an item from the cart or reduces its quantity.
        /// </summary>
        /// <param name="id">The cart ID.</param>
        /// <param name="itemSku">The SKU of the item to remove.</param>
        /// <param name="quantity">The quantity to remove (default is 1).</param>
        /// <returns>The updated cart DTO or a bad request result.</returns>
        [HttpDelete("{id}/items/{itemSku}")]
        public async Task<IActionResult> RemoveItemFromCart(string id, int itemSku, [FromQuery] int quantity = 1)
        {
            var result = await _cartService.RemoveItemFromCartAsync(id, itemSku, quantity);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            var dto = CartMapper.ToDto(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Clears all items from the cart.
        /// </summary>
        /// <param name="id">The cart ID.</param>
        /// <returns>The cleared cart DTO or a bad request result.</returns>
        [HttpDelete("{id}/clear")]
        public async Task<IActionResult> ClearCart(string id)
        {
            var result = await _cartService.ClearCartAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            var dto = CartMapper.ToDto(result.Data);
            return Ok(dto);
        }

        /// <summary>
        /// Deletes a cart by its ID.
        /// </summary>
        /// <param name="id">The cart ID.</param>
        /// <returns>No content if successful, or not found result.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCart(string id)
        {
            var result = await _cartService.DeleteCartAsync(id);
            return result.IsSuccess
                ? NoContent()
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Gets the total price of all items in the cart.
        /// </summary>
        /// <param name="id">The cart ID.</param>
        /// <returns>The total price or a not found result.</returns>
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
