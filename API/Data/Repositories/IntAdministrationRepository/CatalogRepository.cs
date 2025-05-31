// Repositories/IntAdministrationRepository/CatalogRepository.cs
using System.Threading.Tasks;
using API.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.IntAdministrationRepository;

public class CatalogRepository
    : SoftRepository<CatalogEntity>, ICatalogRepository
{
    public CatalogRepository(ApplicationDbContext ctx)
        : base(ctx)
    {
    }

    public Task<List<CatalogCategoryEntity>> GetCatalogCategoriesAsync(
        int catalogId, int skip = 0, int take = 50)
    {
        return _ctx.CatalogCategoryEntities
            .AsNoTracking()
            .Where(cc => cc.CatalogId == catalogId)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task AddCatalogCategoryAsync(CatalogCategoryEntity pivot)
    {
        await _ctx.CatalogCategoryEntities.AddAsync(pivot);
        await _ctx.SaveChangesAsync();
    }

    public async Task RemoveCatalogCategoryAsync(CatalogCategoryEntity pivot)
    {
        _ctx.CatalogCategoryEntities.Remove(pivot);
        await _ctx.SaveChangesAsync();
    }
}