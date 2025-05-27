namespace softserve.projectlabs.Shared.DTOs.Order
{
    public class OrderItemDto
    {
        public int Sku { get; set; }
        public int ItemId { get; set; } 
        public string ItemName { get; set; } 
        public string ItemDescription { get; set; } 
        public decimal UnitPrice { get; set; } 
        public int Quantity { get; set; } 
        public string Category { get; set; } 
        public string Manufacturer { get; set; } 
        public string ImageUrl { get; set; } 
        public decimal TotalPrice => Quantity * UnitPrice; 
    }
}
