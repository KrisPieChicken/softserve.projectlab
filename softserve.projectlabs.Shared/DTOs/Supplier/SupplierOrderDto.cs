using softserve.projectlabs.Shared.DTOs.Order;

namespace softserve.projectlabs.Shared.DTOs.Supplier
{
    public class SupplierOrderDto
    {
        public int OrderId { get; set; }
        public int SupplierId { get; set; }        
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }
}