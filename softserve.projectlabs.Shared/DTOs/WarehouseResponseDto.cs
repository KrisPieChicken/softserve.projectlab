namespace softserve.projectlabs.Shared.DTOs
{
    public class WarehouseResponseDto
    {
        public int WarehouseId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Capacity { get; set; }
        public List<ItemDto> Items { get; set; } = new();
        public int BranchId { get; set; }
    }
}