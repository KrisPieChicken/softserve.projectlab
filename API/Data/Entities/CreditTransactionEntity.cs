using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CreditTransactionEntity
{
    public string Id { get; set; } = null!;

    public string TransactionType { get; set; } = null!;

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime TransactionDate { get; set; }

    public int LineOfCreditId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual LineOfCreditEntity LineOfCredit { get; set; } = null!;
}
