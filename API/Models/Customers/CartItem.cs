using API.Models.IntAdmin;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Customers
{
    /// <summary>
    /// Represents an item in the cart with its quantity.
    /// </summary>
    public class CartItem
    {
        [ForeignKey("Item")]
        public string ItemId { get; set; }

        [ForeignKey("Cart")]
        public string CartId { get; set; }

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