using API.Models.IntAdmin;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.DTOs.Permission;

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
        /// <param name="permissionDto">Permission object to add</param>
        /// <returns>HTTP response with the created permission or error message</returns>
        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] PermissionDto permissionDto)
        {
            var result = await _permissionService.CreatePermissionAsync(permissionDto);
            return result.IsSuccess 
                ? Ok(result.Data) 
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing permission.
        /// </summary>
        /// <param name="permissionId">The ID of the permission to update.</param>
        /// <param name="permissionDto">The updated permission data.</param>
        /// <returns>HTTP response with the updated permission or error message</returns>
        [HttpPut("{PermissionId}")]
        public async Task<IActionResult> UpdatePermission(int permissionId, [FromBody] PermissionDto permissionDto)
        {
            permissionDto.PermissionId = permissionId;
            var result = await _permissionService.UpdatePermissionAsync(permissionDto);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a permission by its unique ID.
        /// </summary>
        /// <param name="permissionId">Unique identifier of the permission</param>
        /// <returns>HTTP response with the permission or error message</returns>
        [HttpGet("{PermissionId}")]
        public async Task<IActionResult> GetPermissionById(int permissionId)
        {
            var result = await _permissionService.GetPermissionByIdAsync(permissionId);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all permissions.
        /// </summary>
        /// <returns>HTTP response with the list of permissions or error message</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllPermissions()
        {
            var result = await _permissionService.GetAllPermissionsAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a permission by its unique ID.
        /// </summary>
        /// <param name="permissionId">Unique identifier of the permission to remove</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpDelete("{permissionId}")]
        public async Task<IActionResult> DeletePermission(int permissionId)
        {
            var result = await _permissionService.DeletePermissionAsync(permissionId);
            return result.IsSuccess
                ? NoContent()
                : NotFound(result.ErrorMessage);
        }
    }
}
