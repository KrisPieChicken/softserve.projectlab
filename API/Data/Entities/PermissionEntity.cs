using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class PermissionEntity
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = null!;

    public string PermissionDescription { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<RolePermissionEntity> RolePermissionEntities { get; set; } = new List<RolePermissionEntity>();
}
