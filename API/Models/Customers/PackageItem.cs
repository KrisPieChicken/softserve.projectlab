using API.Models.IntAdmin;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Customers
{
    /// <summary>
    /// Represents an item in a package with its quantity.
    /// </summary>
    public class PackageItem
    {
        [ForeignKey("Item")]
        public string ItemId { get; set; }

        [ForeignKey("Package")]
        public string PackageId { get; set; }

        /// <summary>
        /// Gets or sets the item.
        /// </summary>
        public Item Item { get; set; } = null!;

        /// <summary>
        /// Gets or sets the quantity of the item.
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or sets any special notes for this item.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// Gets or sets the warranty period in months for this item.
        /// </summary>
        public int? WarrantyMonths { get; set; }

        /// <summary>
        /// Gets or sets the serial number of the item if applicable.
        /// </summary>
        public string? SerialNumber { get; set; }
    }
}