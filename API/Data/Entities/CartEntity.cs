using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CartEntity
{
    public int CartId { get; set; }
    public int CustomerId { get; set; }
    public virtual CustomerEntity Customer { get; set; } = null!;

    public virtual ICollection<CartItemEntity> CartItems { get; set; } = new List<CartItemEntity>();
}


