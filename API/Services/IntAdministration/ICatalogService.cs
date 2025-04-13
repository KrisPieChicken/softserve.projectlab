using API.Models;
using API.Models.IntAdmin;
using softserve.projectlabs.Shared.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;

namespace API.Services.IntAdmin
{
    public interface ICatalogService
    {
        Task<Result<Catalog>> CreateCatalogAsync(CatalogDto catalogDto);
        Task<Result<Catalog>> UpdateCatalogAsync(int catalogId, CatalogDto catalogDto);
        Task<Result<Catalog>> GetCatalogByIdAsync(int catalogId);
        Task<Result<List<Catalog>>> GetAllCatalogsAsync();
        Task<Result<bool>> DeleteCatalogAsync(int catalogId);

        // Category management
        Task<Result<bool>> AddCategoriesToCatalogAsync(int catalogId, List<int> categoryIds);
        Task<Result<bool>> RemoveCategoryFromCatalogAsync(int catalogId, int categoryId);
    }
}
