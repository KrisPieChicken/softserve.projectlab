using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class UserRoleEntity
{
    public int UserId { get; set; }

    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    public virtual RoleEntity Role { get; set; } = null!;

    public virtual UserEntity User { get; set; } = null!;
}
