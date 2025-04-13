using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class SupplierEntity
{
    public int SupplierId { get; set; }

    public string SupplierName { get; set; } = null!;

    public string SupplierAddress { get; set; } = null!;

    public string SupplierContactNumber { get; set; } = null!;

    public string SupplierContactEmail { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<SupplierItemEntity> SupplierItemEntities { get; set; } = new List<SupplierItemEntity>();
}
