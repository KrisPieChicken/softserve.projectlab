using API.Implementations.Domain;
using API.Models;
using softserve.projectlabs.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using API.Models.IntAdmin;

namespace API.Services.IntAdmin
{
    public class CatalogService : ICatalogService
    {
        private readonly CatalogDomain _catalogDomain;
        public CatalogService(CatalogDomain catalogDomain)
        {
            _catalogDomain = catalogDomain;
        }
        public async Task<Result<Catalog>> CreateCatalogAsync(CatalogDto catalogDto)
        {
            return await _catalogDomain.CreateCatalogAsync(catalogDto);
        }
        public async Task<Result<Catalog>> UpdateCatalogAsync(int catalogId, CatalogDto catalogDto)
        {
            return await _catalogDomain.UpdateCatalogAsync(catalogId, catalogDto);
        }
        public async Task<Result<Catalog>> GetCatalogByIdAsync(int catalogId)
        {
            return await _catalogDomain.GetCatalogByIdAsync(catalogId);
        }
        public async Task<Result<List<Catalog>>> GetAllCatalogsAsync()
        {
            return await _catalogDomain.GetAllCatalogsAsync();
        }
        public async Task<Result<bool>> DeleteCatalogAsync(int catalogId)
        {
            return await _catalogDomain.DeleteCatalogAsync(catalogId);
        }
        public async Task<Result<bool>> AddCategoriesToCatalogAsync(int catalogId, List<int> categoryIds)
        {
            return await _catalogDomain.AddCategoriesToCatalogAsync(catalogId, categoryIds);
        }
        public async Task<Result<bool>> RemoveCategoryFromCatalogAsync(int catalogId, int categoryId)
        {
            return await _catalogDomain.RemoveCategoryFromCatalogAsync(catalogId, categoryId);
        }
    }
}
