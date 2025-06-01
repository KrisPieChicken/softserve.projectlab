using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.Data.Entities;
using API.Data.Repositories.BaseRepository;
using Microsoft.EntityFrameworkCore;
using API.Data.Repositories.IntAdministrationRepository.Interfaces;

namespace API.Data.Repositories.IntAdministrationRepository;

public class CatalogRepository : SoftRepository<CatalogEntity>, ICatalogRepository
{
    public CatalogRepository(ApplicationDbContext ctx)
        : base(ctx)
    {
    }

    /// <summary>
    /// Obtiene las categorías asociadas a un catálogo.
    /// Observa que CatalogCategoryEntity NO hereda de BaseEntity, 
    /// así que no filtramos por IsDeleted en el pivote.
    /// </summary>
    public async Task<List<CatalogCategoryEntity>> GetCatalogCategoriesAsync(
        int catalogId, int skip = 0, int take = 50)
    {
        return await _ctx.CatalogCategoryEntities
            .AsNoTracking()
            .Where(cc => cc.CatalogId == catalogId)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    /// <summary>
    /// Agrega una relación CatalogCategoryEntity (pivote).
    /// </summary>
    public async Task AddCatalogCategoryAsync(CatalogCategoryEntity pivot)
    {
        await _ctx.CatalogCategoryEntities.AddAsync(pivot);
        await _ctx.SaveChangesAsync();
    }

    /// <summary>
    /// Elimina físicamente la relación pivote.
    /// Si más adelante quisieras soft‐delete en esa tabla pivote,
    /// tendrías que hacer que CatalogCategoryEntity herede de BaseEntity
    /// y marcar pivot.IsDeleted = true en vez de Remove(pivot).
    /// </summary>
    public async Task RemoveCatalogCategoryAsync(CatalogCategoryEntity pivot)
    {
        _ctx.CatalogCategoryEntities.Remove(pivot);
        await _ctx.SaveChangesAsync();
    }
}
