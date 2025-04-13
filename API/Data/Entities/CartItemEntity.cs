using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CartItemEntity
{
    public int CartId { get; set; }

    public int Sku { get; set; }

    public int ItemQuantity { get; set; }

    public virtual CartEntity Cart { get; set; } = null!;

    public virtual ItemEntity SkuNavigation { get; set; } = null!;
}
