using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CustomerEntity
{
    public int CustomerId { get; set; }
    public required string CustomerType { get; set; }
    public required string CustomerName { get; set; }
    public required string CustomerContactNumber { get; set; }
    public required string CustomerContactEmail { get; set; }

    public virtual ICollection<CartEntity> CartEntities { get; set; } = new List<CartEntity>();
    public virtual ICollection<OrderEntity> OrderEntities { get; set; } = new List<OrderEntity>();
    public virtual LineOfCreditEntity? LineOfCreditEntity { get; set; }
}
