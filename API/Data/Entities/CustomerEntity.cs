using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CustomerEntity : BaseEntity
{
    public int CustomerId { get; set; }
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public string CustomerContactNumber { get; set; } = null!;
    public string CustomerContactEmail { get; set; } = null!;
    public DateOnly? BirthDate { get; set; }
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
    public virtual ICollection<CartEntity> CartEntities { get; set; } = new List<CartEntity>();
    public virtual ICollection<OrderEntity> OrderEntities { get; set; } = new List<OrderEntity>();
}
