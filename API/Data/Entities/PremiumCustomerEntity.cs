using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class PremiumCustomerEntity
{
    public int CustomerId { get; set; }

    public decimal DiscountRate { get; set; }

    public DateTime MembershipStartDate { get; set; }

    public DateTime MembershipExpiryDate { get; set; }

    public string TierLevel { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual CustomerEntity Customer { get; set; } = null!;
}
