using API.Data.Repositories.IntAdministrationRepository.Interfaces;
using API.Data.Repositories.LogisticsRepositories.Interfaces;
using API.Models.Logistics.Supplier;
using softserve.projectlabs.Shared.Utilities;

namespace API.Implementations.Domain
{
    public class SupplierDomain
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IItemRepository _itemRepository;

        public SupplierDomain(ISupplierRepository supplierRepository, IItemRepository itemRepository)
        {
            _supplierRepository = supplierRepository;
            _itemRepository = itemRepository;
        }

        public async Task<Result<Supplier>> CreateSupplier(Supplier supplier)
        {
            try
            {
                var entity = supplier.ToEntity();
                await _supplierRepository.AddAsync(entity);
                return Result<Supplier>.Success(entity.ToDomain());
            }
            catch (Exception ex)
            {
                return Result<Supplier>.Failure($"Failed to create supplier: {ex.Message}");
            }
        }

        public async Task<Result<Supplier>> GetSupplierByIdAsync(int supplierId)
        {
            try
            {
                var entity = await _supplierRepository.GetByIdAsync(supplierId);
                if (entity == null)
                    return Result<Supplier>.Failure("Supplier not found.");

                return Result<Supplier>.Success(entity.ToDomain());
            }
            catch (Exception ex)
            {
                return Result<Supplier>.Failure($"Failed to get supplier: {ex.Message}");
            }
        }

        public async Task<Result<List<Supplier>>> GetAllSuppliersAsync()
        {
            try
            {
                var entities = await _supplierRepository.GetAllAsync();
                var suppliers = entities.Select(e => e.ToDomain()).ToList();
                return Result<List<Supplier>>.Success(suppliers);
            }
            catch (Exception ex)
            {
                return Result<List<Supplier>>.Failure($"Failed to retrieve suppliers: {ex.Message}");
            }
        }

        public async Task<Result<Supplier>> UpdateSupplier(Supplier updatedSupplier)
        {
            try
            {
                var entity = await _supplierRepository.GetByIdAsync(updatedSupplier.SupplierId);
                if (entity == null)
                    return Result<Supplier>.Failure("Supplier not found.");

                entity.SupplierName = updatedSupplier.Name;
                entity.SupplierContactNumber = updatedSupplier.ContactNumber;
                entity.SupplierContactEmail = updatedSupplier.ContactEmail;
                entity.SupplierAddress = updatedSupplier.Address;
                entity.UpdatedAt = DateTime.UtcNow;

                await _supplierRepository.UpdateAsync(entity);

                return Result<Supplier>.Success(entity.ToDomain());
            }
            catch (Exception ex)
            {
                return Result<Supplier>.Failure($"Failed to update supplier: {ex.Message}");
            }
        }

        public async Task<Result<bool>> RemoveSupplierAsync(int supplierId)
        {
            try
            {
                await _supplierRepository.DeleteAsync(supplierId);
                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to remove supplier: {ex.Message}");
            }
        }

        public async Task<bool> SupplierHasItem(int supplierId, int sku)
        {
            return await _supplierRepository.SupplierHasItem(supplierId, sku);
            throw new NotImplementedException();
        }

        public async Task<Result<bool>> LinkItemToSupplier(int supplierId, int sku)
        {
            var supplier = await _supplierRepository.GetByIdAsync(supplierId);
            if (supplier == null)
                return Result<bool>.Failure("Supplier not found.");


            var item = await _itemRepository.GetBySkuAsync(sku);
            if (item == null)
                return Result<bool>.Failure("Item not found.");

            var linkExists = await SupplierHasItem(supplierId, sku);
            if (linkExists)
                return Result<bool>.Failure("Item is already linked to this supplier.");

            await _supplierRepository.LinkItemToSupplier(supplierId, sku);

            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> UndeleteSupplierAsync(int supplierId)
        {
            try
            {
                var entity = await _supplierRepository.GetByIdAsync(supplierId);
                if (entity == null)
                    return Result<bool>.Failure("Supplier not found.");

                if (!entity.IsDeleted)
                    return Result<bool>.Failure("Supplier is already active.");

                entity.IsDeleted = false;
                entity.UpdatedAt = DateTime.UtcNow;
                await _supplierRepository.UpdateAsync(entity);

                return Result<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to restore supplier: {ex.Message}");
            }
        }

    }
}
