using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class BusinessCustomerEntity
{
    public int CustomerId { get; set; }

    public string CompanyName { get; set; } = null!;

    public string TaxId { get; set; } = null!;

    public string Industry { get; set; } = null!;

    public int EmployeeCount { get; set; }

    public decimal AnnualRevenue { get; set; }

    public string BusinessSize { get; set; } = null!;

    public decimal VolumeDiscountRate { get; set; }

    public string CreditTerms { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual CustomerEntity Customer { get; set; } = null!;
}
