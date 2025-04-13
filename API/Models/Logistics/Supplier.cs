using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;
using API.Models.Logistics;
using API.Models;
using softserve.projectlabs.Shared.Utilities;

namespace Logistics.Models
{
    public class Supplier : ISupplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string SupplierAddress { get; set; } = null!;
        public string SupplierContactNumber { get; set; } = null!;
        public string SupplierContactEmail { get; set; } = null!;
        public List<Item> ProductsSupplied { get; set; } = new List<Item>();
        public bool IsActive { get; set; }
        public List<SupplierOrder> Orders { get; set; } = new List<SupplierOrder>();

        // Parameterless constructor for Dependency Injection (DI) & serialization
        public Supplier() { }

        public Supplier(int supplierId, string supplierName, string supplierAddress, string supplierContactNumber, string supplierContactEmail)
        {
            SupplierId = supplierId;
            SupplierName = supplierName;
            SupplierAddress = supplierAddress;
            SupplierContactNumber = supplierContactNumber;
            SupplierContactEmail = supplierContactEmail;
            IsActive = true;
        }
        /// <summary>
        /// Add a new supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public Result<ISupplier> AddSupplier(ISupplier supplier)
        {
            // Logic for adding a supplier
            return Result<ISupplier>.Success(supplier);
        }
        /// <summary>
        /// Update an existing supplier
        /// </summary>
        /// <param name="supplier"></param>
        /// <returns></returns>
        public Result<ISupplier> UpdateSupplier(ISupplier supplier)
        {
            // Logic to update an existing supplier
            return Result<ISupplier>.Success(supplier);
        }
        /// <summary>
        /// Get a supplier by ID
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public Result<ISupplier> GetSupplierById(int supplierId)
        {
            var supplier = new Supplier(supplierId, "Sample Supplier", "Sample Address", "123-456-7890", "contact@sample.com");
            return Result<ISupplier>.Success(supplier);
        }
        /// <summary>
        /// Get all suppliers
        /// </summary>
        /// <returns></returns>
        public Result<List<ISupplier>> GetAllSuppliers()
        {
            var suppliers = new List<ISupplier>
            {
                new Supplier(1, "Sample Supplier 1", "Sample Address", "123-456-7890", "contact1@sample.com"),
                new Supplier(2, "Sample Supplier 2", "Sample Address", "123-456-7890", "contact2@sample.com")
            };
            return Result<List<ISupplier>>.Success(suppliers);
        }
        /// <summary>
        /// Delete a supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public Result<bool> DeleteSupplier(int supplierId)
        {
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Add a product to the supplier's inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<bool> AddProductToSupplier(Item item)
        {
            if (ProductsSupplied.Any(p => p.Sku == item.Sku))
                return Result<bool>.Failure("Product already supplied by this supplier.");

            ProductsSupplied.Add(item);
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Remove a product from supplier's inventory
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public Result<bool> RemoveProductFromSupplier(Item item)
        {
            var existingItem = ProductsSupplied.FirstOrDefault(p => p.Sku == item.Sku);
            if (existingItem == null)
                return Result<bool>.Failure("Product not found in supplier inventory.");

            ProductsSupplied.Remove(existingItem);
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Get all products a supplier provides
        /// </summary>
        /// <returns></returns>
        public Result<List<Item>> GetSupplierProducts()
        {
            return Result<List<Item>>.Success(ProductsSupplied);
        }

        /// <summary>
        /// Place an order with the supplier
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        public Result<SupplierOrder> PlaceOrder(int supplierId, List<Item> items)
        {
            var order = new SupplierOrder(supplierId, items);
            Orders.Add(order);
            return Result<SupplierOrder>.Success(order);
        }

        /// <summary>
        /// Get all orders placed with this supplier
        /// </summary>
        /// <returns></returns>
        public Result<List<SupplierOrder>> GetSupplierOrders()
        {
            return Result<List<SupplierOrder>>.Success(Orders);
        }

        /// <summary>
        /// Cancel a supplier order
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public Result<bool> CancelOrder(int orderId)
        {
            var order = Orders.FirstOrDefault(o => o.OrderId == orderId);
            if (order == null)
                return Result<bool>.Failure("Order not found.");

            Orders.Remove(order);
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Check if the supplier is active
        /// </summary>
        /// <returns></returns>
        public Result<bool> CheckSupplierAvailability()
        {
            return Result<bool>.Success(IsActive);
        }

        /// <summary>
        /// Update supplier active/inactive status
        /// </summary>
        /// <param name="isActive"></param>
        /// <returns></returns>
        public Result<bool> UpdateSupplierStatus(bool isActive)
        {
            IsActive = isActive;
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Rate a supplier and provide feedback
        /// </summary>
        /// <param name="rating"></param>
        /// <param name="feedback"></param>
        /// <returns></returns>
        public Result<bool> RateSupplier(int rating, string feedback)
        {
            // Save rating and feedback logic (could be stored in DB)
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Get supplier rating
        /// </summary>
        /// <returns></returns>
        public Result<int> GetSupplierRating()
        {
            // Return mock rating for now
            return Result<int>.Success(5); // Assume 5-star rating
        }
    }
}
