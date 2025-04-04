using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class WarehouseItemEntity
{
    public int WarehouseId { get; set; }
    public int Sku { get; set; }
    public int ItemQuantity { get; set; }

    public virtual WarehouseEntity Warehouse { get; set; } = null!;
    public virtual ItemEntity Item { get; set; } = null!;
}
