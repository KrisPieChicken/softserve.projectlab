using softserve.projectlabs.Shared.DTOs;
using API.Models.IntAdmin;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.IntAdmin
{
    /// <summary>
    /// API Controller for managing Permission operations.
    /// </summary>
    [ApiController]
    [Route("api/permissions")]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionService _permissionService;

        /// <summary>
        /// Constructor with dependency injection for IPermissionService.
        /// </summary>
        public PermissionController(IPermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        /// <summary>
        /// Adds a new permission.
        /// </summary>
        /// <param name="permission">Permission object to add</param>
        /// <returns>HTTP response with the created permission or error message</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto permissionDto)
        {
            var permission = ConvertToPermission(permissionDto);
            var result = await _permissionService.CreatePermissionAsync(permission);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="permission">Permission object with updated data</param>
        /// <returns>HTTP response with the updated permission or error message</returns>
        [HttpPut("{PermissionId}")]
        public async Task<IActionResult> UpdatePermission(int PermissionId, [FromBody] Permission permission)
        {
            permission.PermissionId = PermissionId;
            var result = await _permissionService.UpdatePermissionAsync(permission);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a permission by its unique ID.
        /// </summary>
        /// <param name="permissionId">Unique identifier of the permission</param>
        /// <returns>HTTP response with the permission or error message</returns>
        [HttpGet("{PermissionId}")]
        public async Task<IActionResult> GetPermissionById(int PermissionId)
        {
            var result = await _permissionService.GetPermissionByIdAsync(PermissionId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all permissions.
        /// </summary>
        /// <returns>HTTP response with the list of permissions or error message</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result = await _permissionService.GetAllPermissionsAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a permission by its unique ID.
        /// </summary>
        /// <param name="permissionId">Unique identifier of the permission to remove</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpDelete("{permissionId}")]
        public async Task<IActionResult> RemovePermission(int permissionId)
        {
            var result = await _permissionService.DeletePermissionAsync(permissionId);
            if (result.IsNoContent)
            {
                return NoContent();
            }
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Convierte el DTO a la entidad Permission.
        /// </summary>
        /// <param name="dto">El DTO recibido</param>
        /// <returns>Una instancia de Permission</returns>
        private Permission ConvertToPermission(PermissionDto dto)
        {
            return new Permission
            {
                PermissionName = dto.PermissionName,
                PermissionDescription = dto.PermissionDescription
                // No se asigna el PermissionId ya que este se gestiona (por la URL o la base de datos)
            };
        }
    }
}
