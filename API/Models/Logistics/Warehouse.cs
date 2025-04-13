using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models;
using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.Logistics
{
    public class Warehouse : IWarehouse
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public List<Item> Items { get; set; } = new List<Item>();
        public int BranchId { get; internal set; }
        public Data.Entities.BranchEntity Branch { get; internal set; }

        // Parameterless constructor for Dependency Injection (DI) & serialization
        public Warehouse() { }

        public Warehouse(int warehouseId, string name, string location, int capacity)
        {
            WarehouseId = warehouseId;
            Name = name;
            Location = location;
            Capacity = capacity;
        }

        public async Task<Result<IWarehouse>> AddItemAsync(Item item)
        {
            await Task.CompletedTask; 
            Items.Add(item);
            return Result<IWarehouse>.Success(this);
        }

        public async Task<Result<IWarehouse>> RemoveItemAsync(Item item)
        {
            await Task.CompletedTask;
            var existingItem = Items.FirstOrDefault(i => i.Sku == item.Sku);
            if (existingItem != null)
            {
                Items.Remove(existingItem);
                return Result<IWarehouse>.Success(this);
            }
            return Result<IWarehouse>.Failure("Item not found in warehouse.");
        }

        public async Task<Result<IWarehouse>> GetAvailableStockAsync(int sku)
        {
            await Task.CompletedTask;
            var stock = Items.FirstOrDefault(i => i.Sku == sku);
            return stock != null
                ? Result<IWarehouse>.Success(this)
                : Result<IWarehouse>.Failure("Item not found.");
        }

        public async Task<Result<bool>> UpdateItemStockAsync(int sku, int quantity)
        {
            await Task.CompletedTask;
            var item = Items.FirstOrDefault(i => i.Sku == sku);
            if (item != null)
            {
                item.CurrentStock += quantity;
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure("Item not found.");
        }

        public async Task<Result<int>> CheckItemStockAsync(int sku)
        {
            await Task.CompletedTask;
            var item = Items.FirstOrDefault(i => i.Sku == sku);
            return item != null
                ? Result<int>.Success(item.CurrentStock)
                : Result<int>.Failure("Item not found");
        }

        public async Task<Result<bool>> IsItemInStockAsync(int sku, int requiredQuantity)
        {
            await Task.CompletedTask;
            var item = Items.FirstOrDefault(i => i.Sku == sku);
            return item != null && item.CurrentStock >= requiredQuantity
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Insufficient stock.");
        }

        public async Task<Result<bool>> TransferItemAsync(int sku, int quantity, IWarehouse targetWarehouse)
        {
            await Task.CompletedTask;
            var item = Items.FirstOrDefault(i => i.Sku == sku);
            if (item != null && item.CurrentStock >= quantity)
            {
                item.CurrentStock -= quantity;
                await targetWarehouse.AddItemAsync(new Item
                {
                    Sku = item.Sku,
                    CurrentStock = quantity
                });
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure("Not enough stock or item not found.");
        }

        public async Task<Result<List<Item>>> GetLowStockItemsAsync(int threshold)
        {
            await Task.CompletedTask;
            var lowStockItems = Items.Where(i => i.CurrentStock <= threshold).ToList();
            return Result<List<Item>>.Success(lowStockItems);
        }

        public async Task<Result<List<Item>>> GetOutOfStockItemsAsync()
        {
            await Task.CompletedTask;
            var outOfStockItems = Items.Where(i => i.CurrentStock == 0).ToList();
            return Result<List<Item>>.Success(outOfStockItems);
        }

        public async Task<Result<bool>> ReserveStockForOrderAsync(int sku, int quantity)
        {
            await Task.CompletedTask;
            var item = Items.FirstOrDefault(i => i.Sku == sku);
            if (item != null && item.CurrentStock >= quantity)
            {
                item.CurrentStock -= quantity;
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure("Insufficient stock for reservation.");
        }

        public async Task<Result<bool>> ReleaseReservedStockAsync(int sku, int quantity)
        {
            await Task.CompletedTask;
            var item = Items.FirstOrDefault(i => i.Sku == sku);
            if (item != null)
            {
                item.CurrentStock += quantity;
                return Result<bool>.Success(true);
            }
            return Result<bool>.Failure("Item not found.");
        }

        public async Task<Result<bool>> ShipItemsAsync(List<Item> items)
        {
            await Task.CompletedTask;
            foreach (var item in items)
            {
                var warehouseItem = Items.FirstOrDefault(i => i.Sku == item.Sku);
                if (warehouseItem == null || warehouseItem.CurrentStock < item.CurrentStock)
                    return Result<bool>.Failure($"Not enough stock for item: {item.ItemName}");

                warehouseItem.CurrentStock -= item.CurrentStock;
            }
            return Result<bool>.Success(true);
        }

        public async Task<Result<decimal>> GetTotalInventoryValueAsync()
        {
            await Task.CompletedTask;
            var totalValue = Items.Sum(i => i.CurrentStock * i.ItemPrice);
            return Result<decimal>.Success(totalValue);
        }

        public async Task<Result<string>> GenerateInventoryReportAsync()
        {
            await Task.CompletedTask;
            var report = System.Text.Json.JsonSerializer.Serialize(Items);
            return Result<string>.Success(report);
        }

        // Sync versions for backwards compatibility (if needed)
        public Result<decimal> GetTotalInventoryValue() => GetTotalInventoryValueAsync().GetAwaiter().GetResult();
        public Result<string> GenerateInventoryReport() => GenerateInventoryReportAsync().GetAwaiter().GetResult();
    }
}