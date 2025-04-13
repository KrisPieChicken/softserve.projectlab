using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class SupplierItemEntity
{
    public int SupplierId { get; set; }

    public int Sku { get; set; }

    public int ItemQuantity { get; set; }

    public virtual ItemEntity SkuNavigation { get; set; } = null!;

    public virtual SupplierEntity Supplier { get; set; } = null!;
}
