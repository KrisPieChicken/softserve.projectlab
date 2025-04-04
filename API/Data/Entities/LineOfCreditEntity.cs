using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class LineOfCreditEntity
{
    public int CustomerId { get; set; }
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }

    public virtual CustomerEntity Customer { get; set; } = null!;
}
