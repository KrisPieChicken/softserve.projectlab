using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.DTOs.Item;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.IntAdmin
{
    /// <summary>
    /// API Controller for managing Item operations.
    /// </summary>
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        /// <summary>
        /// Constructor with dependency injection for IItemService.
        /// </summary>
        /// <param name="itemService">The item service instance</param>
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Creates a new item.
        /// </summary>
        /// <param name="itemDto">Item data transfer object containing creation information</param>
        /// <returns>HTTP response with the created item or error message</returns>
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemCreateDto itemDto)
        {
            var result = await _itemService.CreateItemAsync(itemDto);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves an item by its unique ID.
        /// </summary>
        /// <param name="itemId">Unique identifier of the item</param>
        /// <returns>HTTP response with the item or error message</returns>
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            var result = await _itemService.GetItemByIdAsync(itemId);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all items.
        /// </summary>
        /// <returns>HTTP response with the list of items or error message</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var result = await _itemService.GetAllItemsAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing item.
        /// </summary>
        /// <param name="itemId">The ID of the item to update</param>
        /// <param name="itemDto">The updated item data</param>
        /// <returns>HTTP response with the updated item or error message</returns>
        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] ItemDto itemDto)
        {
            var result = await _itemService.UpdateItemAsync(itemId, itemDto);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Removes an item by its unique ID. (Using soft delete)
        /// </summary>
        /// <param name="itemId">Unique identifier of the item to remove</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var result = await _itemService.DeleteItemAsync(itemId);
            return result.IsSuccess
                ? NoContent()
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Updates the price of an existing item.
        /// </summary>
        /// <param name="itemId">The ID of the item whose price will be updated</param>
        /// <param name="newPrice">The new price value</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpPatch("{itemId}/price")]
        public async Task<IActionResult> UpdatePrice(int itemId, [FromBody] decimal newPrice)
        {
            var result = await _itemService.UpdatePriceAsync(itemId, newPrice);
            return result.IsSuccess
                ? Ok("Price updated successfully")
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates the discount of an existing item.
        /// </summary>
        /// <param name="itemId">The ID of the item whose discount will be updated</param>
        /// <param name="newDiscount">The new discount value</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpPatch("{itemId}/discount")]
        public async Task<IActionResult> UpdateDiscount(int itemId, [FromBody] decimal newDiscount)
        {
            var result = await _itemService.UpdateDiscountAsync(itemId, newDiscount);
            return result.IsSuccess
                ? Ok("Discount updated successfully")
                : BadRequest(result.ErrorMessage);
        }
    }
}
