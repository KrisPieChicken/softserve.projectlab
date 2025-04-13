using API.Models;
using API.Models.Customers;
using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.Logistics
{
    /// <summary>
    /// Represents an order with details such as customer, items, status, and total amount.
    /// </summary>
    public class Order : IOrder
    {
        /// <summary>
        /// Gets or sets the order ID.
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// Gets or sets the customer ID.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Gets or sets the customer details.
        /// </summary>
        public Customer Customer { get; set; }

        /// <summary>
        /// Gets or sets the order date.
        /// </summary>
        public DateTime OrderDate { get; set; }

        /// <summary>
        /// Gets or sets the list of items in the order.
        /// </summary>
        public List<Item> Items { get; set; }

        /// <summary>
        /// Gets or sets the status of the order.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the total amount of the order.
        /// </summary>
        public decimal TotalAmount { get; set; }

        [NotMapped]
        public List<OrderItemRequest> OrderLineItems { get; set; } = new();

        // Legacy compatible property
        //public List<Item> Items
        //{
        //    get => OrderLineItems.Select(oi => new Item
        //    {
        //        Sku = oi.Sku,
        //        Quantity = oi.Quantity
        //    }).ToList();
        //    set => OrderLineItems = value?.Select(i => new OrderItemRequest
        //    {
        //        Sku = i.Sku,
        //        Quantity = i.Quantity
        //    }).ToList() ?? new List<OrderItemRequest>();
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="Order"/> class.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <param name="customerId">The customer ID.</param>
        /// <param name="orderDate">The order date.</param>
        /// <param name="items">The list of items in the order.</param>
        /// <param name="status">The status of the order.</param>

        // Parameterless constructor for Dependency Injection (DI) & serialization
        public Order(){}

        public Order(int orderId, string customerId, DateTime orderDate, List<Item> items, string status)
        {
            OrderId = orderId;
            CustomerId = customerId;
            OrderDate = orderDate;
            Items = items;
            Status = status;
        }

        /// <summary>
        /// Adds a new order.
        /// </summary>
        /// <param name="order">The order to add.</param>
        /// <returns>A result containing the added order.</returns>
        public Result<IOrder> AddOrder(IOrder order)
        {
            // Logic for adding a new order (e.g., saving to database or collection)
            return Result<IOrder>.Success(order);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="order">The order to update.</param>
        /// <returns>A result containing the updated order.</returns>
        public Result<IOrder> UpdateOrder(IOrder order)
        {
            // Logic for updating an existing order
            return Result<IOrder>.Success(order);
        }

        /// <summary>
        /// Retrieves an order by its ID.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <returns>A result containing the order with the specified ID.</returns>
        public Result<IOrder> GetOrderById(int orderId)
        {
            // Logic to retrieve an order by its ID
            var order = new Order(orderId, "customerId_123", DateTime.Now, new List<Item>(), "Pending")
            {
                Customer = GetCustomerById("customerId_123") // Retrieve the customer using their ID
            };
            return Result<IOrder>.Success(order);
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>A result containing a list of all orders.</returns>
        public Result<List<IOrder>> GetAllOrders()
        {
            // Logic to get all orders
            var orders = new List<IOrder>
                {
                    new Order(1, "customerId_123", DateTime.Now, new List<Item>(), "Shipped")
                    {
                        Customer = GetCustomerById("customerId_123")
                    },
                    new Order(2, "customerId_456", DateTime.Now.AddDays(-1), new List<Item>(), "Pending")
                    {
                        Customer = GetCustomerById("customerId_456")
                    }
                };
            return Result<List<IOrder>>.Success(orders);
        }

        /// <summary>
        /// Removes an order by its ID.
        /// </summary>
        /// <param name="orderId">The order ID.</param>
        /// <returns>A result indicating whether the removal was successful.</returns>
        public Result<bool> RemoveOrder(int orderId)
        {
            // Logic to remove an order
            return Result<bool>.Success(true); // Assume the order was removed successfully
        }

        /// <summary>
        /// Adds an item to the order.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>A result indicating whether the addition was successful.</returns>
        public Result<bool> AddItemToOrder(Item item)
        {
            // Logic to add an item to an order
            Items.Add(item);
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Removes an item from the order.
        /// </summary>
        /// <param name="item">The item to remove.</param>
        /// <returns>A result indicating whether the removal was successful.</returns>
        public Result<bool> RemoveItemFromOrder(Item item)
        {
            // Logic to remove an item from an order
            Items.Remove(item);
            return Result<bool>.Success(true);
        }

        /// <summary>
        /// Retrieves the customer by their ID.
        /// </summary>
        /// <param name="customerId">The customer ID.</param>
        /// <returns>The customer with the specified ID.</returns>
        private Customer GetCustomerById(string customerId)
        {
            // You can implement the logic here to retrieve the customer by their ID.
            // For example, fetch from a database or another service.
            return new Customer { Id = customerId, FirstName = "John", LastName = "Doe" };
        }
    }
}
