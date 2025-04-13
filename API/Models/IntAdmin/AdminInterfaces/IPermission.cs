using API.Data.Entities;
using System.Collections.Generic;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin.Interfaces
{
    public interface IPermission
    {
        int PermissionId { get; set; }
        string PermissionName { get; set; }
        string PermissionDescription { get; set; }
    }
}
