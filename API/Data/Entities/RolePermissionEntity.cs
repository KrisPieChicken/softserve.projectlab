using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class RolePermissionEntity
{
    public int RoleId { get; set; }

    public int PermissionId { get; set; }

    public string RoleName { get; set; } = null!;

    public string PermissionName { get; set; } = null!;

    public virtual PermissionEntity Permission { get; set; } = null!;

    public virtual RoleEntity Role { get; set; } = null!;
}
