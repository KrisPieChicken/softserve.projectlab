using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class OrderEntity
{
    public int OrderId { get; set; }
    public int CustomerId { get; set; }
    public decimal OrderTotalAmount { get; set; }
    public required string OrderStatus { get; set; }
    public DateTime OrderDate { get; set; }

    public virtual CustomerEntity Customer { get; set; } = null!;
    public virtual ICollection<OrderItemEntity> OrderItemEntities { get; set; } = new List<OrderItemEntity>();
}
