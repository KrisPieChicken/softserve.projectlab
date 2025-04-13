namespace softserve.projectlabs.Shared.DTOs
{
    public class OrderDto : BaseDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
