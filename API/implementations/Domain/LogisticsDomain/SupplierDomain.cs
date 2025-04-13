using API.Data.Entities;
using API.Models;
using Logistics.Models;
using softserve.projectlabs.Shared.Utilities;


namespace API.Implementations.Domain
{
    public class SupplierDomain
    {
        private readonly List<Supplier> _suppliers = new List<Supplier>(); // Example in-memory storage for suppliers

        /// <summary>
        /// Creates a new supplier.
        /// </summary>
        /// <param name="supplier">The supplier to create.</param>
        /// <returns>A result containing the created supplier or an error message.</returns>
        public async Task<Result<Supplier>> CreateSupplier(Supplier supplier)
        {
            try
            {
                _suppliers.Add(supplier);
                return Result<Supplier>.Success(supplier);
            }
            catch (Exception ex)
            {
                return Result<Supplier>.Failure($"Failed to create supplier: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves a supplier by their ID.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier to retrieve.</param>
        /// <returns>A result containing the supplier or an error message.</returns>
        public async Task<Result<Supplier>> GetSupplierById(int supplierId)
        {
            try
            {
                var supplier = _suppliers.FirstOrDefault(s => s.SupplierId == supplierId);
                return supplier != null ? Result<Supplier>.Success(supplier) : Result<Supplier>.Failure("Supplier not found.");
            }
            catch (Exception ex)
            {
                return Result<Supplier>.Failure($"Failed to retrieve supplier: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves all suppliers.
        /// </summary>
        /// <returns>A result containing the list of suppliers or an error message.</returns>
        public async Task<Result<List<Supplier>>> GetAllSuppliers()
        {
            try
            {
                return Result<List<Supplier>>.Success(_suppliers);
            }
            catch (Exception ex)
            {
                return Result<List<Supplier>>.Failure($"Failed to retrieve suppliers: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing supplier.
        /// </summary>
        /// <param name="supplier">The supplier with updated information.</param>
        /// <returns>A result containing the updated supplier or an error message.</returns>
        public async Task<Result<Supplier>> UpdateSupplier(Supplier supplier)
        {
            try
            {
                var existingSupplier = _suppliers.FirstOrDefault(s => s.SupplierId == supplier.SupplierId);
                if (existingSupplier != null)
                {
                    existingSupplier.SupplierName = supplier.SupplierName;
                    existingSupplier.SupplierName = supplier.SupplierName;
                    existingSupplier.SupplierName = supplier.SupplierName;
                    // Update any other properties here
                    return Result<Supplier>.Success(existingSupplier);
                }
                else
                {
                    return Result<Supplier>.Failure("Supplier not found.");
                }
            }
            catch (Exception ex)
            {
                return Result<Supplier>.Failure($"Failed to update supplier: {ex.Message}");
            }
        }

        /// <summary>
        /// Removes a supplier by their ID.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier to remove.</param>
        /// <returns>A result indicating success or failure.</returns>
        public async Task<Result<bool>> RemoveSupplier(int supplierId)
        {
            try
            {
                var supplierToRemove = _suppliers.FirstOrDefault(s => s.SupplierId == supplierId);
                if (supplierToRemove != null)
                {
                    _suppliers.Remove(supplierToRemove);
                    return Result<bool>.Success(true);
                }
                else
                {
                    return Result<bool>.Failure("Supplier not found.");
                }
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Failed to remove supplier: {ex.Message}");
            }
        }
    }
}
