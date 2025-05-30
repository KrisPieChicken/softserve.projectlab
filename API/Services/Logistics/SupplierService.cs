using API.Implementations.Domain;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using API.Models.Logistics.Supplier;
using softserve.projectlabs.Shared.DTOs.Supplier;

namespace API.Services.Logistics
{    
    public class SupplierService : ISupplierService
    {
        private readonly SupplierDomain _supplierDomain;

        public SupplierService(SupplierDomain supplierDomain)
        {
            _supplierDomain = supplierDomain;
        }

        public async Task<Result<SupplierDto>> CreateSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = supplierDto.ToDomain();
            var domainResult = await _supplierDomain.CreateSupplier(supplier);
            if (!domainResult.IsSuccess)
                return Result<SupplierDto>.Failure(domainResult.ErrorMessage);

            return Result<SupplierDto>.Success(domainResult.Data.ToDto());
        }

        public async Task<Result<SupplierDto>> GetSupplierByIdAsync(int supplierId)
        {
            var domainResult = await _supplierDomain.GetSupplierByIdAsync(supplierId);
            if (!domainResult.IsSuccess)
                return Result<SupplierDto>.Failure(domainResult.ErrorMessage);

            return Result<SupplierDto>.Success(domainResult.Data.ToDto());
        }

        public async Task<Result<List<SupplierDto>>> GetAllSuppliersAsync()
        {
            var domainResult = await _supplierDomain.GetAllSuppliersAsync();
            if (!domainResult.IsSuccess)
                return Result<List<SupplierDto>>.Failure(domainResult.ErrorMessage);

            var supplierDtos = domainResult.Data.Select(s => s.ToDto()).ToList();
            return Result<List<SupplierDto>>.Success(supplierDtos);
        }

        public async Task<Result<SupplierDto>> UpdateSupplierAsync(SupplierDto supplierDto)
        {
            var supplier = supplierDto.ToDomain();
            var domainResult = await _supplierDomain.UpdateSupplier(supplier);
            if (!domainResult.IsSuccess)
                return Result<SupplierDto>.Failure(domainResult.ErrorMessage);

            return Result<SupplierDto>.Success(domainResult.Data.ToDto());
        }

        public async Task<Result<bool>> DeleteSupplierAsync(int supplierId)
        {
            return await _supplierDomain.RemoveSupplierAsync(supplierId);
        }

        public async Task<Result<bool>> UndeleteSupplierAsync(int supplierId)
        {
            return await _supplierDomain.UndeleteSupplierAsync(supplierId);
        }

        public async Task<Result<bool>> AddItemToSupplierAsync(int supplierId, int sku)
        {
            var result = await _supplierDomain.LinkItemToSupplier(supplierId, sku);
            return result;
        }
    }
}
