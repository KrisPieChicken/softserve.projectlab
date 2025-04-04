using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class WarehouseEntity
{
    public int WarehouseId { get; set; }
    public required string WarehouseLocation { get; set; }
    public int WarehouseCapacity { get; set; }

    public int BranchId { get; set; }
    public virtual BranchEntity Branch { get; set; } = null!;
    public virtual ICollection<WarehouseItemEntity> WarehouseItemEntities { get; set; } = new List<WarehouseItemEntity>();
}
