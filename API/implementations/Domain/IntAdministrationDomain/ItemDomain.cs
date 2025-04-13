using API.Data;
using API.Data.Entities;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using Microsoft.EntityFrameworkCore;
using softserve.projectlabs.Shared.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Implementations.Domain
{
    public class ItemDomain
    {
        private readonly ApplicationDbContext _context;

        public ItemDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Creates a new item in the database.
        /// </summary>
        public async Task<Result<Item>> CreateItemAsync(ItemDto itemDto)
        {
            try
            {
                var itemEntity = new ItemEntity
                {
                    Sku = itemDto.Sku,
                    ItemName = itemDto.ItemName,
                    ItemDescription = itemDto.ItemDescription,
                    OriginalStock = itemDto.OriginalStock,
                    CurrentStock = itemDto.CurrentStock,
                    ItemCurrency = itemDto.ItemCurrency,
                    ItemUnitCost = itemDto.ItemUnitCost,
                    ItemMarginGain = itemDto.ItemMarginGain,
                    ItemDiscount = itemDto.ItemDiscount,
                    ItemAdditionalTax = itemDto.ItemAdditionalTax,
                    ItemPrice = itemDto.ItemPrice,
                    ItemStatus = itemDto.ItemStatus,
                    CategoryId = itemDto.CategoryId,
                    ItemImage = itemDto.ItemImage
                };

                _context.ItemEntities.Add(itemEntity);
                await _context.SaveChangesAsync();

                var item = await MapToItem(itemEntity.ItemId);
                return Result<Item>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<Item>.Failure($"Error creating item: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing item in the database.
        /// </summary>
        public async Task<Result<Item>> UpdateItemAsync(int itemId, ItemDto itemDto)
        {
            try
            {
                var itemEntity = await _context.ItemEntities.FirstOrDefaultAsync(i => i.ItemId == itemId);
                if (itemEntity == null)
                    return Result<Item>.Failure("Item not found.");

                itemEntity.Sku = itemDto.Sku;
                itemEntity.ItemName = itemDto.ItemName;
                itemEntity.ItemDescription = itemDto.ItemDescription;
                itemEntity.OriginalStock = itemDto.OriginalStock;
                itemEntity.CurrentStock = itemDto.CurrentStock;
                itemEntity.ItemCurrency = itemDto.ItemCurrency;
                itemEntity.ItemUnitCost = itemDto.ItemUnitCost;
                itemEntity.ItemMarginGain = itemDto.ItemMarginGain;
                itemEntity.ItemDiscount = itemDto.ItemDiscount;
                itemEntity.ItemAdditionalTax = itemDto.ItemAdditionalTax;
                itemEntity.ItemPrice = itemDto.ItemPrice;
                itemEntity.ItemStatus = itemDto.ItemStatus;
                itemEntity.CategoryId = itemDto.CategoryId;
                itemEntity.ItemImage = itemDto.ItemImage;

                await _context.SaveChangesAsync();

                var updatedItem = await MapToItem(itemId);
                return Result<Item>.Success(updatedItem);
            }
            catch (Exception ex)
            {
                return Result<Item>.Failure($"Error updating item: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an item by its ID.
        /// </summary>
        public async Task<Result<Item>> GetItemByIdAsync(int itemId)
        {
            try
            {
                var item = await MapToItem(itemId);
                if (item == null)
                    return Result<Item>.Failure("Item not found.");
                return Result<Item>.Success(item);
            }
            catch (Exception ex)
            {
                return Result<Item>.Failure($"Error retrieving item: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all items from the database.
        /// </summary>
        public async Task<Result<List<Item>>> GetAllItemsAsync()
        {
            try
            {
                var itemEntities = await _context.ItemEntities.ToListAsync();
                var items = new List<Item>();
                foreach (var entity in itemEntities)
                {
                    var item = await MapToItem(entity.ItemId);
                    if (item != null)
                        items.Add(item);
                }
                return Result<List<Item>>.Success(items);
            }
            catch (Exception ex)
            {
                return Result<List<Item>>.Failure($"Error retrieving items: {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an item by its ID from the database.
        /// </summary>
        public async Task<Result<bool>> DeleteItemAsync(int itemId)
        {
            try
            {
                var itemEntity = await _context.ItemEntities.FirstOrDefaultAsync(i => i.ItemId == itemId);
                if (itemEntity == null)
                    return Result<bool>.Failure("Item not found.");

                _context.ItemEntities.Remove(itemEntity);
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error deleting item: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the price of the item.
        /// </summary>
        public async Task<Result<bool>> UpdatePriceAsync(int itemId, decimal newPrice)
        {
            try
            {
                var itemEntity = await _context.ItemEntities.FirstOrDefaultAsync(i => i.ItemId == itemId);
                if (itemEntity == null)
                    return Result<bool>.Failure("Item not found.");
                itemEntity.ItemPrice = newPrice;
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error updating price: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates the discount of the item.
        /// </summary>
        public async Task<Result<bool>> UpdateDiscountAsync(int itemId, decimal? newDiscount)
        {
            try
            {
                var itemEntity = await _context.ItemEntities.FirstOrDefaultAsync(i => i.ItemId == itemId);
                if (itemEntity == null)
                    return Result<bool>.Failure("Item not found.");
                itemEntity.ItemDiscount = newDiscount;
                await _context.SaveChangesAsync();
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Error updating discount: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps an ItemEntity to the domain model Item.
        /// </summary>
        private async Task<Item> MapToItem(int itemId)
        {
            var itemEntity = await _context.ItemEntities.FirstOrDefaultAsync(i => i.ItemId == itemId);
            if (itemEntity == null)
                return null;

            var item = new Item
            {
                ItemId = itemEntity.ItemId,
                Sku = itemEntity.Sku,
                ItemName = itemEntity.ItemName,
                ItemDescription = itemEntity.ItemDescription,
                OriginalStock = itemEntity.OriginalStock,
                CurrentStock = itemEntity.CurrentStock,
                ItemCurrency = itemEntity.ItemCurrency,
                ItemUnitCost = itemEntity.ItemUnitCost,
                ItemMarginGain = itemEntity.ItemMarginGain,
                ItemDiscount = itemEntity.ItemDiscount,
                ItemAdditionalTax = itemEntity.ItemAdditionalTax,
                ItemPrice = itemEntity.ItemPrice,
                ItemStatus = itemEntity.ItemStatus,
                CategoryId = itemEntity.CategoryId,
                ItemImage = itemEntity.ItemImage
            };
            return item;
        }
    }
}
