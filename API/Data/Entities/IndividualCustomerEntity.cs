using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class IndividualCustomerEntity
{
    public int CustomerId { get; set; }

    public bool IsEligibleForPromotions { get; set; }

    public string CommunicationPreference { get; set; } = null!;

    public int LoyaltyPoints { get; set; }

    public DateTime? LastPurchaseDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual CustomerEntity Customer { get; set; } = null!;
}
