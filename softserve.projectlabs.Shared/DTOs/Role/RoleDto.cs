﻿namespace softserve.projectlabs.Shared.DTOs.Role
{
    public class RoleDto : BaseDto
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; } = string.Empty;
        public string RoleDescription { get; set; } = string.Empty;
        public bool RoleStatus { get; set; }

        // List of permission IDs to be assigned to the role 
        public List<int> PermissionIds { get; set; } = new List<int>();
    }
}
