namespace softserve.projectlabs.Shared.DTOs.Order
{
    public class OrderDto : BaseDto
    {
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string OrderStatus { get; set; } // Add this property
        public List<OrderItemDto> Items { get; set; }
    }
}
