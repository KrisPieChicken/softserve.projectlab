namespace softserve.projectlabs.Shared.DTOs.Order
{
    public class OrderItemRequestDto
    {
        public int ItemId { get; set; }
        public int Quantity { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}
