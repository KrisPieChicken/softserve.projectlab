using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class PermissionEntity
{
    public int PermissionId { get; set; }
    public string PermissionName { get; set; } = null!;
    public string PermissionDescription { get; set; } = null!;

    public virtual ICollection<RolePermissionEntity> RolePermissionEntities { get; set; } = new List<RolePermissionEntity>();
}
