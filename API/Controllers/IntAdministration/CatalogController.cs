using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Authorization;
using API.Utils.Extensions;

namespace API.Controllers.IntAdmin
{
    /// <summary>
    /// API Controller for managing Catalog operations.
    /// </summary>
    [ApiController]
    [Route("api/catalogs")]
    [Authorize]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        /// <summary>
        /// Constructor with dependency injection for ICatalogService.
        /// </summary>
        /// <param name="catalogService">The catalog service instance</param>
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Creates a new catalog.
        /// </summary>
        /// <param name="catalogDto">Catalog data transfer object containing creation information</param>
        /// <returns>HTTP response with the created catalog or error message</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCatalog([FromBody] CatalogCreateDto catalogDto)
        {
            var result = await _catalogService.CreateCatalogAsync(catalogDto);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a catalog by its unique ID.
        /// </summary>
        /// <param name="catalogId">Unique identifier of the catalog</param>
        /// <returns>HTTP response with the catalog or error message</returns>
        [HttpGet("{catalogId}")]
        public async Task<IActionResult> GetCatalogById(int catalogId)
        {
            var result = await _catalogService.GetCatalogByIdAsync(catalogId);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all catalogs.
        /// </summary>
        /// <returns>HTTP response with the list of catalogs or error message</returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCatalogs()
        {
            var result = await _catalogService.GetAllCatalogsAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing catalog.
        /// </summary>
        /// <param name="catalogId">The ID of the catalog to update</param>
        /// <param name="catalogDto">The updated catalog data</param>
        /// <returns>HTTP response with the updated catalog or error message</returns>
        [HttpPut("{catalogId}")]
        public async Task<IActionResult> UpdateCatalog(int catalogId, [FromBody] CatalogUpdateDto catalogDto)
        {
            var result = await _catalogService.UpdateCatalogAsync(catalogId, catalogDto);
            return result.ToActionResult(data => Ok(data));
        }

        /// <summary>
        /// Removes a catalog by its unique ID. (Using soft delete)
        /// </summary>
        /// <param name="catalogId">Unique identifier of the catalog to remove</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpDelete("{catalogId}")]
        public async Task<IActionResult> DeleteCatalog(int catalogId)
        {
            var result = await _catalogService.DeleteCatalogAsync(catalogId);
            return result.IsSuccess
                ? NoContent()
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Adds categories to an existing catalog.
        /// </summary>
        /// <param name="catalogId">The ID of the catalog to add categories to</param>
        /// <param name="categoryIds">List of category IDs to add to the catalog</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpPost("{catalogId}/categories")]
        public async Task<IActionResult> AddCategoriesToCatalog(int catalogId, [FromBody] List<int> categoryIds)
        {
            var result = await _catalogService.AddCategoriesToCatalogAsync(catalogId, categoryIds);
            return result.IsSuccess
                ? Ok("Categories added successfully.")
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes a category from an existing catalog.
        /// </summary>
        /// <param name="catalogId">The ID of the catalog</param>
        /// <param name="categoryId">The ID of the category to remove</param>
        /// <returns>HTTP response indicating success or failure</returns>
        [HttpDelete("{catalogId}/categories/{categoryId}")]
        public async Task<IActionResult> RemoveCategoryFromCatalog(int catalogId, int categoryId)
        {
            var result = await _catalogService.RemoveCategoryFromCatalogAsync(catalogId, categoryId);
            return result.IsSuccess
                ? Ok("Category removed successfully.")
                : NotFound(result.ErrorMessage);
        }
    }
}