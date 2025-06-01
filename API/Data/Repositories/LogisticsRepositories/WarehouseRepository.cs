using API.Data;
using API.Data.Entities;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repositories.LogisticsRepositories
{
    public class WarehouseRepository : IWarehouseRepository
    {
        private readonly ApplicationDbContext _context;

        public WarehouseRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<WarehouseEntity?> GetByIdAsync(int warehouseId)
        {
            return await _context.WarehouseEntities
                .Include(w => w.WarehouseItemEntities)
                .ThenInclude(wi => wi.SkuNavigation)
                .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId && !w.IsDeleted);
        }

        public async Task<List<WarehouseEntity>> GetAllAsync()
        {
            return await _context.WarehouseEntities
                .Where(w => !w.IsDeleted)
                .Include(w => w.WarehouseItemEntities)
                .ThenInclude(wi => wi.SkuNavigation)
                .ToListAsync();
        }

        public async Task AddAsync(WarehouseEntity warehouseEntity)
        {
            _context.WarehouseEntities.Add(warehouseEntity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(WarehouseEntity warehouseEntity)
        {
            _context.WarehouseEntities.Update(warehouseEntity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int warehouseId)
        {
            var warehouse = await GetByIdAsync(warehouseId);
            if (warehouse != null)
            {
                warehouse.IsDeleted = true;
                _context.WarehouseEntities.Update(warehouse);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<WarehouseEntity?> GetByNameAsync(string name)
        {
            return await _context.WarehouseEntities
                .FirstOrDefaultAsync(w => w.WarehouseLocation == name && !w.IsDeleted);
        }

        public async Task UndeleteAsync(int warehouseId)
        {
            var warehouse = await _context.WarehouseEntities
                .IgnoreQueryFilters() 
                .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId);

            if (warehouse != null && warehouse.IsDeleted)
            {
                warehouse.IsDeleted = false;
                _context.WarehouseEntities.Update(warehouse);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<WarehouseEntity?> GetByIdIncludingDeletedAsync(int warehouseId)
        {
            return await _context.WarehouseEntities
                .Include(w => w.WarehouseItemEntities)
                .ThenInclude(wi => wi.SkuNavigation)
                .FirstOrDefaultAsync(w => w.WarehouseId == warehouseId);
        }


    }
}

