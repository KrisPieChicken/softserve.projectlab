namespace API.Models.Logistics
{
    public class OrderItemResponse
    {
        public int Sku { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
