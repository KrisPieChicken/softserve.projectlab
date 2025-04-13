using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service interface for item operations.
    /// </summary>
    public interface IItemService
    {
        Task<Result<Item>> CreateItemAsync(ItemDto itemDto);
        Task<Result<Item>> UpdateItemAsync(int itemId, ItemDto itemDto);
        Task<Result<Item>> GetItemByIdAsync(int itemId);
        Task<Result<List<Item>>> GetAllItemsAsync();
        Task<Result<bool>> DeleteItemAsync(int itemId);

        // Optional methods for updating price and discount.
        Task<Result<bool>> UpdatePriceAsync(int itemId, decimal newPrice);
        Task<Result<bool>> UpdateDiscountAsync(int itemId, decimal? newDiscount);
    }
}
