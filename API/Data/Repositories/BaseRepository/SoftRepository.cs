using API.Data.Repositories.BaseRepository;
using API.Data;
using Microsoft.EntityFrameworkCore;
using API.Data.Entities;

public class SoftRepository<T> : ISoftRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _ctx;
    protected readonly DbSet<T> _dbSet;

    public SoftRepository(ApplicationDbContext ctx)
    {
        _ctx = ctx;
        _dbSet = ctx.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        var e = await _dbSet.FindAsync(id);
        return e != null && !e.IsDeleted ? e : null;
    }

    public async Task<IEnumerable<T>> GetAllAsync(int skip = 0, int take = 50)
        => await _dbSet
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .Skip(skip)
            .Take(take)
            .ToListAsync();

    public async Task<T> AddAsync(T entity)
    {
        entity.CreatedAt = entity.UpdatedAt = DateTime.UtcNow;
        await _dbSet.AddAsync(entity);
        await _ctx.SaveChangesAsync();
        return entity;
    }

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(entity);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var e = await _dbSet.FindAsync(id);
        if (e == null) return;
        e.IsDeleted = true;
        e.UpdatedAt = DateTime.UtcNow;
        _dbSet.Update(e);
        await _ctx.SaveChangesAsync();
    }
}
