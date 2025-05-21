using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs.Catalog;
using Microsoft.AspNetCore.Authorization;
using API.Utils.Extensions;

namespace API.Controllers.IntAdmin
{
    [ApiController]
    [Route("api/catalogs")]
    [Authorize]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        /// <summary>
        /// Creates a new Catalog.
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> CreateCatalog([FromBody] CatalogCreateDto catalogDto)
        {
            var result = await _catalogService.CreateCatalogAsync(catalogDto);
            return result.IsSuccess 
                ? Ok(result.Data) 
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Obtains an existing Catalog by ID.
        /// </summary>
        [HttpGet("{catalogId}")]
        public async Task<IActionResult> GetCatalogById(int catalogId)
        {
            var result = await _catalogService.GetCatalogByIdAsync(catalogId);
            return result.IsSuccess 
                ? Ok(result.Data) 
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Obtains all existing Catalogs.
        /// </summary>
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
        /// Updates an existing Catalog.
        /// </summary>
        [HttpPut("{catalogId}")]
        public async Task<IActionResult> UpdateCatalog(int catalogId, [FromBody] CatalogUpdateDto catalogDto)
        {
            var result = await _catalogService.UpdateCatalogAsync(catalogId, catalogDto);
            return result.ToActionResult(data => Ok(data));
        }

        /// <summary>
        /// Deletes an existing Catalog by ID. (Using soft delete)
        /// </summary>
        [HttpDelete("{catalogId}")]
        public async Task<IActionResult> DeleteCatalog(int catalogId)
        {
            var result = await _catalogService.DeleteCatalogAsync(catalogId);
            return result.IsSuccess
                ? NoContent()
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Adds Categories to an existing Catalog.
        /// </summary>
        [HttpPost("{catalogId}/categories")]
        public async Task<IActionResult> AddCategoriesToCatalog(int catalogId, [FromBody] List<int> categoryIds)
        {
            var result = await _catalogService.AddCategoriesToCatalogAsync(catalogId, categoryIds);

            return result.ToActionResult(_ => Ok("Categories added successfully."));
        }

        /// <summary>
        /// Removes a Category from an existing Catalog.
        /// </summary>
        [HttpDelete("{catalogId}/categories/{categoryId}")]
        public async Task<IActionResult> RemoveCategoryFromCatalog(int catalogId, int categoryId)
        {
            var result = await _catalogService.RemoveCategoryFromCatalogAsync(catalogId, categoryId);
            return result.ToActionResult(_ => Ok("Category removed successfully."));
        }
    }
}
