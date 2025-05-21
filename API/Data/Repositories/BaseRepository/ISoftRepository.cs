using API.Data.Entities;

namespace API.Data.Repositories.BaseRepository;

public interface ISoftRepository<T> where T : BaseEntity
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync(int skip = 0, int take = 50);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id); // soft-delete
}
