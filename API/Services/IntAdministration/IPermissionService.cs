﻿using API.Data.Entities;
using API.Models.IntAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service interface for permission operations.
    /// </summary>
    public interface IPermissionService
    {
        Task<Result<Permission>> AddPermissionAsync(Permission permission);
        Task<Result<Permission>> UpdatePermissionAsync(Permission permission);
        Task<Result<Permission>> GetPermissionByIdAsync(int permissionId);
        Task<Result<List<Permission>>> GetAllPermissionsAsync();
        Task<Result<bool>> RemovePermissionAsync(int permissionId);
    }
}
