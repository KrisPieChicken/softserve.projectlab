using API.Models.IntAdmin;
using softserve.projectlabs.Shared.DTOs.Warehouse;

namespace API.Models.Logistics.Warehouses
{
    public static class WarehouseMapper
    {
        public static Warehouse ToDomain(this WarehouseDto dto, List<Item>? items = null)
            => new Warehouse(
                dto.WarehouseId,
                dto.Name,
                dto.Location,
                dto.Capacity,
                dto.BranchId,
                items);

        public static WarehouseDto ToDto(this Warehouse warehouse)
            => new WarehouseDto
            {
                WarehouseId = warehouse.WarehouseId,
                Name = warehouse.Name,
                Location = warehouse.Location,
                Capacity = warehouse.Capacity,
                BranchId = warehouse.BranchId
            };
    }
}
