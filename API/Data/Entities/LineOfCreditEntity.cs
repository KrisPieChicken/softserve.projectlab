using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class LineOfCreditEntity
{
    public int CustomerId { get; set; }

    public decimal CreditLimit { get; set; }

    public decimal CurrentBalance { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CreditTransactionEntity> CreditTransactionEntities { get; set; } = new List<CreditTransactionEntity>();

    public virtual CustomerEntity Customer { get; set; } = null!;
}
