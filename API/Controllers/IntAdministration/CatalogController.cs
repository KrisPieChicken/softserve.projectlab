using softserve.projectlabs.Shared.DTOs;
using API.Services.IntAdmin;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers.IntAdmin
{
    [ApiController]
    [Route("api/catalogs")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;
        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCatalog([FromBody] CatalogDto catalogDto)
        {
            var result = await _catalogService.CreateCatalogAsync(catalogDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        [HttpPut("{catalogId}")]
        public async Task<IActionResult> UpdateCatalog(int catalogId, [FromBody] CatalogDto catalogDto)
        {
            var result = await _catalogService.UpdateCatalogAsync(catalogId, catalogDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpGet("{catalogId}")]
        public async Task<IActionResult> GetCatalogById(int catalogId)
        {
            var result = await _catalogService.GetCatalogByIdAsync(catalogId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCatalogs()
        {
            var result = await _catalogService.GetAllCatalogsAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{catalogId}")]
        public async Task<IActionResult> DeleteCatalog(int catalogId)
        {
            var result = await _catalogService.DeleteCatalogAsync(catalogId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        [HttpPost("{catalogId}/add-categories")]
        public async Task<IActionResult> AddCategoriesToCatalog(int catalogId, [FromBody] List<int> categoryIds)
        {
            var result = await _catalogService.AddCategoriesToCatalogAsync(catalogId, categoryIds);
            return result.IsSuccess ? Ok("Categories added successfully.") : BadRequest(result.ErrorMessage);
        }

        [HttpDelete("{catalogId}/remove-category/{categoryId}")]
        public async Task<IActionResult> RemoveCategoryFromCatalog(int catalogId, int categoryId)
        {
            var result = await _catalogService.RemoveCategoryFromCatalogAsync(catalogId, categoryId);
            return result.IsSuccess ? Ok("Category removed successfully.") : NotFound(result.ErrorMessage);
        }
    }
}
