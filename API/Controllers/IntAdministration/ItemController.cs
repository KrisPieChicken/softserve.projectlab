using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.IntAdmin
{
    [ApiController]
    [Route("api/items")]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;

        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        /// <summary>
        /// Creates a new item using the provided DTO.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemDto itemDto)
        {
            var result = await _itemService.CreateItemAsync(itemDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing item using the provided DTO.
        /// </summary>
        [HttpPut("{itemId}")]
        public async Task<IActionResult> UpdateItem(int itemId, [FromBody] ItemDto itemDto)
        {
            var result = await _itemService.UpdateItemAsync(itemId, itemDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves an item by its ID.
        /// </summary>
        [HttpGet("{itemId}")]
        public async Task<IActionResult> GetItemById(int itemId)
        {
            var result = await _itemService.GetItemByIdAsync(itemId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all items.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var result = await _itemService.GetAllItemsAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Deletes an item by its ID.
        /// </summary>
        [HttpDelete("{itemId}")]
        public async Task<IActionResult> DeleteItem(int itemId)
        {
            var result = await _itemService.DeleteItemAsync(itemId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Updates the price of an item.
        /// </summary>
        [HttpPut("update-price/{itemId}")]
        public async Task<IActionResult> UpdatePrice(int itemId, [FromBody] decimal newPrice)
        {
            var result = await _itemService.UpdatePriceAsync(itemId, newPrice);
            return result.IsSuccess ? Ok("Price updated successfully") : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates the discount of an item.
        /// </summary>
        [HttpPut("update-discount/{itemId}")]
        public async Task<IActionResult> UpdateDiscount(int itemId, [FromBody] decimal? newDiscount)
        {
            var result = await _itemService.UpdateDiscountAsync(itemId, newDiscount);
            return result.IsSuccess ? Ok("Discount updated successfully") : BadRequest(result.ErrorMessage);
        }
    }
}
