using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class OrderItemEntity
{
    public int OrderId { get; set; }

    public int Sku { get; set; }

    public int ItemQuantity { get; set; }

    public virtual OrderEntity Order { get; set; } = null!;

    public virtual ItemEntity SkuNavigation { get; set; } = null!;
}
