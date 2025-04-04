using System.Collections.Generic;

namespace API.Data.Entities;

public partial class BranchEntity
{
    public int BranchId { get; set; }

    public required string BranchName { get; set; }

    public required string BranchCity { get; set; }

    public required string BranchAddress { get; set; }

    public required string BranchRegion { get; set; }

    public required string BranchContactNumber { get; set; }

    public required string BranchContactEmail { get; set; }

    public virtual ICollection<UserEntity> UsersEntities { get; set; } = new List<UserEntity>();

    public virtual ICollection<WarehouseEntity> WarehouseEntities { get; set; } = new List<WarehouseEntity>();
}
