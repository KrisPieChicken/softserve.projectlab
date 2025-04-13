using API.Models.IntAdmin;

namespace API.Models.Customers
{
    /// <summary>
    /// Represents a shopping cart for a customer with items and quantities.
    /// </summary>
    public class Cart
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the customer associated with this cart.
        /// </summary>
        public Customer Customer { get; set; } = new Customer();

        /// <summary>
        /// Gets or sets the collection of cart items with their quantities.
        /// </summary>
        public List<CartItem> Items { get; set; } = new List<CartItem>();

        /// <summary>
        /// Gets or sets the creation date of the cart.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets or sets the last update date of the cart.
        /// </summary>
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Gets the total price of all items in the cart.
        /// </summary>
        /// <returns>The sum of all item prices multiplied by their quantities.</returns>
        public decimal GetTotalPrice()
        {
            return Items.Sum(item => item.Item.ItemUnitCost * item.Quantity);
        }

        /// <summary>
        /// Adds an item to the cart or increases its quantity if it already exists.
        /// </summary>
        /// <param name="item">The item to add to the cart.</param>
        /// <param name="quantity">The quantity to add. Must be positive.</param>
        /// <exception cref="ArgumentException">Thrown when the quantity is not positive.</exception>
        public void AddItem(Item item, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
            }

            var existingItem = Items.FirstOrDefault(i => i.Item == item);
            
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem { Item = item, Quantity = quantity });
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Removes an item from the cart or reduces its quantity.
        /// </summary>
        /// <param name="itemId">The ID of the item to remove.</param>
        /// <param name="quantity">The quantity to remove. Must be positive.</param>
        /// <exception cref="ArgumentException">Thrown when the quantity is not positive.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the item is not found in the cart.</exception>
        public void RemoveItem(string itemId, int quantity)
        {
            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be positive", nameof(quantity));
            }

            var existingItem = Items.FirstOrDefault(i => i.Item.Sku.ToString() == itemId);
            
            if (existingItem == null)
            {
                throw new InvalidOperationException($"Item with ID {itemId} not found in cart");
            }

            if (existingItem.Quantity <= quantity)
            {
                Items.Remove(existingItem);
            }
            else
            {
                existingItem.Quantity -= quantity;
            }

            UpdatedAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Clears all items from the cart.
        /// </summary>
        public void Clear()
        {
            Items.Clear();
            UpdatedAt = DateTime.UtcNow;
        }
    }

    /// <summary>
    /// Represents an item in the cart with its quantity.
    /// </summary>
    public class CartItem
    {
        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public Item Item { get; set; } = null!;

        /// <summary>
        /// Gets or sets the quantity of the item.
        /// </summary>
        public int Quantity { get; set; }
    }
}