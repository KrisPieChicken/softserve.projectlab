using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class BranchEntity
{
    public int BranchId { get; set; }

    public string BranchName { get; set; } = null!;

    public string BranchCity { get; set; } = null!;

    public string BranchAddress { get; set; } = null!;

    public string BranchRegion { get; set; } = null!;

    public string BranchContactNumber { get; set; } = null!;

    public string BranchContactEmail { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<UserEntity> UserEntities { get; set; } = new List<UserEntity>();

    public virtual ICollection<WarehouseEntity> WarehouseEntities { get; set; } = new List<WarehouseEntity>();
}
