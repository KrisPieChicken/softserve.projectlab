using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CustomerEntity
{
    public int CustomerId { get; set; }

    public string CustomerType { get; set; } = null!;

    public string CustomerName { get; set; } = null!;

    public string CustomerContactNumber { get; set; } = null!;

    public string CustomerContactEmail { get; set; } = null!;

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public DateOnly? BirthDate { get; set; }

    public string? Email { get; set; }

    public string? PhoneNumber { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? State { get; set; }

    public string? ZipCode { get; set; }

    public DateTime RegistrationDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual BusinessCustomerEntity? BusinessCustomerEntity { get; set; }

    public virtual ICollection<CartEntity> CartEntities { get; set; } = new List<CartEntity>();

    public virtual IndividualCustomerEntity? IndividualCustomerEntity { get; set; }

    public virtual LineOfCreditEntity? LineOfCreditEntity { get; set; }

    public virtual ICollection<OrderEntity> OrderEntities { get; set; } = new List<OrderEntity>();

    public virtual PremiumCustomerEntity? PremiumCustomerEntity { get; set; }
}
