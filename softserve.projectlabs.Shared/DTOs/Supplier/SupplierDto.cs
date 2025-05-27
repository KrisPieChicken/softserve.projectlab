namespace softserve.projectlabs.Shared.DTOs.Supplier
{
    public class SupplierDto : BaseDto
    {
        public int SupplierId { get; set; }
        public string Name { get; set; }
        public string ContactNumber { get; set; }
        public string ContactEmail { get; set; }
        public string Address { get; set; }
    }
}
