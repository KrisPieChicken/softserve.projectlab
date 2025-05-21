using API.Data.Entities;
using API.Data.Repositories.BaseRepository;
using System.Collections.Generic;
using System.Threading.Tasks;

public interface ICatalogRepository : ISoftRepository<CatalogEntity>
{
    Task<List<CatalogCategoryEntity>> GetCatalogCategoriesAsync(
        int catalogId, int skip = 0, int take = 50);

    Task AddCatalogCategoryAsync(CatalogCategoryEntity pivot);
    Task RemoveCatalogCategoryAsync(CatalogCategoryEntity pivot);
}
