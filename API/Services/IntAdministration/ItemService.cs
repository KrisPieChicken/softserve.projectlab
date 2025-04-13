using API.Implementations.Domain;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service class for item operations. Delegates business logic to the ItemDomain.
    /// </summary>
    public class ItemService : IItemService
    {
        private readonly ItemDomain _itemDomain;

        public ItemService(ItemDomain itemDomain)
        {
            _itemDomain = itemDomain;
        }

        public async Task<Result<Item>> CreateItemAsync(ItemDto itemDto)
        {
            return await _itemDomain.CreateItemAsync(itemDto);
        }

        public async Task<Result<Item>> UpdateItemAsync(int itemId, ItemDto itemDto)
        {
            return await _itemDomain.UpdateItemAsync(itemId, itemDto);
        }

        public async Task<Result<Item>> GetItemByIdAsync(int itemId)
        {
            return await _itemDomain.GetItemByIdAsync(itemId);
        }

        public async Task<Result<List<Item>>> GetAllItemsAsync()
        {
            return await _itemDomain.GetAllItemsAsync();
        }

        public async Task<Result<bool>> DeleteItemAsync(int itemId)
        {
            return await _itemDomain.DeleteItemAsync(itemId);
        }

        public async Task<Result<bool>> UpdatePriceAsync(int itemId, decimal newPrice)
        {
            return await _itemDomain.UpdatePriceAsync(itemId, newPrice);
        }

        public async Task<Result<bool>> UpdateDiscountAsync(int itemId, decimal? newDiscount)
        {
            return await _itemDomain.UpdateDiscountAsync(itemId, newDiscount);
        }
    }
}
