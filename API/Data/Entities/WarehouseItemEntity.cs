using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class WarehouseItemEntity
{
    public int WarehouseId { get; set; }

    public int Sku { get; set; }

    public int ItemQuantity { get; set; }

    public virtual ItemEntity SkuNavigation { get; set; } = null!;

    public virtual WarehouseEntity Warehouse { get; set; } = null!;
}
