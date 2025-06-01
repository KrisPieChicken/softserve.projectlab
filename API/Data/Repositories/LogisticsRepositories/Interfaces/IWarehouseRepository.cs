using API.Data.Entities;

namespace API.Data.Repositories.LogisticsRepositories.Interfaces
{
    public interface IWarehouseRepository
    {
        Task<WarehouseEntity?> GetByIdAsync(int warehouseId);
        Task<List<WarehouseEntity>> GetAllAsync();
        Task AddAsync(WarehouseEntity warehouseEntity);
        Task UpdateAsync(WarehouseEntity warehouseEntity);
        Task DeleteAsync(int warehouseId);
        Task<WarehouseEntity?> GetByNameAsync(string name);
        Task UndeleteAsync(int warehouseId);
        Task<WarehouseEntity?> GetByIdIncludingDeletedAsync(int warehouseId);

    }
}
