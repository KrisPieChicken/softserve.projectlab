using API.Data.Entities;
using API.Models.IntAdmin.Interfaces;
using System.Collections.Generic;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin

{
    public class Role : IRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
        public bool RoleStatus { get; set; }

        public List<IPermission> Permissions { get; set; } = new List<IPermission>();
    }
}
