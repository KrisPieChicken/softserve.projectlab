
namespace API.Models.Logistics.Order
{
    public class OrderItemRequest
    {
        public int Sku { get; set; }
        public int Quantity { get; set; } // Order-specific quantity
        public List<Warehouse.Warehouse.ItemRequest> Items { get; set; } = new List<Warehouse.Warehouse.ItemRequest>();
    }
}
