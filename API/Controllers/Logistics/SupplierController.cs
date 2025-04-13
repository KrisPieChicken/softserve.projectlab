using API.Services.Logistics;
using Logistics.Models;
using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;
using AutoMapper; // Add this for mapping

namespace API.Controllers.Logistics
{
    /// <summary>
    /// Controller for managing supplier-related operations.
    /// </summary>
    [Route("api/suppliers")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierController"/> class.
        /// </summary>
        /// <param name="supplierService">Service for supplier operations.</param>
        /// <param name="mapper">Mapper for DTO conversions.</param>
        public SupplierController(ISupplierService supplierService, IMapper mapper)
        {
            _supplierService = supplierService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new supplier.
        /// </summary>
        /// <param name="supplier">The supplier to create.</param>
        /// <returns>HTTP 201 Created if successful, otherwise HTTP 400 Bad Request.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] Supplier supplier)
        {
            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            var result = await _supplierService.CreateSupplierAsync(supplierDto);
            if (!result.IsSuccess)
                return BadRequest(result.ErrorMessage);

            return CreatedAtAction(nameof(GetSupplierById), new { supplierId = result.Data.SupplierId }, result.Data);
        }

        /// <summary>
        /// Retrieves a supplier by its ID.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier to retrieve.</param>
        /// <returns>HTTP 200 OK with the supplier data, or HTTP 404 Not Found if not found.</returns>
        [HttpGet("{supplierId}")]
        public async Task<IActionResult> GetSupplierById(int supplierId)
        {
            var result = await _supplierService.GetSupplierByIdAsync(supplierId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all suppliers.
        /// </summary>
        /// <returns>HTTP 200 OK with the list of suppliers, or HTTP 404 Not Found if none exist.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllSuppliers()
        {
            var result = await _supplierService.GetAllSuppliersAsync();
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing supplier.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier to update.</param>
        /// <param name="supplier">The updated supplier data.</param>
        /// <returns>HTTP 200 OK with the updated supplier data, or HTTP 404 Not Found if not found.</returns>
        [HttpPut("{supplierId}")]
        public async Task<IActionResult> UpdateSupplier(int supplierId, [FromBody] Supplier supplier)
        {
            var supplierDto = _mapper.Map<SupplierDto>(supplier);
            supplierDto.SupplierId = supplierId;
            var result = await _supplierService.UpdateSupplierAsync(supplierDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Deletes a supplier by its ID.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier to delete.</param>
        /// <returns>HTTP 204 No Content if successful, or HTTP 404 Not Found if not found.</returns>
        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> DeleteSupplier(int supplierId)
        {
            var result = await _supplierService.DeleteSupplierAsync(supplierId);
            if (result.IsNoContent)
            {
                return NoContent();
            }
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
    }
}

