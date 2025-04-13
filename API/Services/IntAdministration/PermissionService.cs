using API.Data.Entities;
using API.Implementations.Domain;
using API.Models;
using API.Models.IntAdmin;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;

namespace API.Services.IntAdmin
{
    /// <summary>
    /// Service class for permission operations. Delegates business logic to the PermissionDomain.
    /// </summary>
    public class PermissionService : IPermissionService
    {
        private readonly PermissionDomain _permissionDomain;

        /// <summary>
        /// Constructor with dependency injection for PermissionDomain.
        /// </summary>
        public PermissionService(PermissionDomain permissionDomain)
        {
            _permissionDomain = permissionDomain;
        }

        public async Task<Result<Permission>> CreatePermissionAsync(Permission permission)
        {
            return await _permissionDomain.CreatePermissionAsync(permission);
        }

        public async Task<Result<Permission>> UpdatePermissionAsync(Permission permission)
        {
            return await _permissionDomain.UpdatePermissionAsync(permission);
        }

        public async Task<Result<Permission>> GetPermissionByIdAsync(int permissionId)
        {
            return await _permissionDomain.GetPermissionByIdAsync(permissionId);
        }

        public async Task<Result<List<Permission>>> GetAllPermissionsAsync()
        {
            return await _permissionDomain.GetAllPermissionsAsync();
        }

        public async Task<Result<bool>> DeletePermissionAsync(int permissionId)
        {
            return await _permissionDomain.DeletePermissionAsync(permissionId);
        }
    }
}
