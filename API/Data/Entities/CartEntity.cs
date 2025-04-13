using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CartEntity
{
    public int CartId { get; set; }

    public int CustomerId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CartItemEntity> CartItemEntities { get; set; } = new List<CartItemEntity>();

    public virtual CustomerEntity Customer { get; set; } = null!;
}
