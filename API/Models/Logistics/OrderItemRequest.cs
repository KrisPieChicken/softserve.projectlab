namespace API.Models.Logistics
{
    public class OrderItemRequest
    {
        public int Sku { get; set; }
        public int Quantity { get; set; } // Order-specific quantity
        public List<ItemRequest> Items { get; set; } = new List<ItemRequest>();
    }
}
