using API.Implementations.Domain;
using API.Models;
using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service class for role operations. Delegates business logic to the RoleDomain.
    /// </summary>
    public class RoleService : IRoleService
    {
        private readonly RoleDomain _roleDomain;

        /// <summary>
        /// Constructor with dependency injection for RoleDomain.
        /// </summary>
        public RoleService(RoleDomain roleDomain)
        {
            _roleDomain = roleDomain;
        }

        public async Task<Result<Role>> CreateRoleAsync(RoleDto roleDto)
        {
            return await _roleDomain.CreateRoleAsync(roleDto);
        }

        public async Task<Result<Role>> UpdateRoleAsync(int roleId, RoleDto roleDto)
        {
            return await _roleDomain.UpdateRoleAsync(roleId, roleDto);
        }

        public async Task<Result<Role>> GetRoleByIdAsync(int roleId)
        {
            return await _roleDomain.GetRoleByIdAsync(roleId);
        }

        public async Task<Result<List<Role>>> GetAllRolesAsync()
        {
            return await _roleDomain.GetAllRolesAsync();
        }

        public async Task<Result<bool>> DeleteRoleAsync(int roleId)
        {
            return await _roleDomain.DeleteRoleAsync(roleId);
        }

        public async Task<Result<bool>> AddPermissionsToRoleAsync(int roleId, List<int> permissionIds)
        {
            return await _roleDomain.AddPermissionsToRoleAsync(roleId, permissionIds);
        }

        public async Task<Result<bool>> RemovePermissionFromRoleAsync(int roleId, int permissionId)
        {
            return await _roleDomain.RemovePermissionFromRoleAsync(roleId, permissionId);
        }
    }
}
