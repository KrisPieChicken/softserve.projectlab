namespace softserve.projectlabs.Shared.DTOs
{
    public class OrderItemDto
    {
        public int ItemId { get; set; } // Unique identifier for the item
        public string ItemName { get; set; } // Name of the item
        public int Quantity { get; set; } // Quantity of the item in the order
        public decimal UnitPrice { get; set; } // Price per unit of the item
        public decimal TotalPrice => Quantity * UnitPrice; // Calculated total price for the item
    }
}
