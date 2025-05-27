using softserve.projectlabs.Shared.DTOs.Item;

namespace softserve.projectlabs.Shared.DTOs.Warehouse
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