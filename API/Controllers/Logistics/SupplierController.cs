using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs.Supplier;

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
        /// <param name="supplierDto">The supplier data to create.</param>
        /// <returns>HTTP 201 Created if successful, otherwise HTTP 400 Bad Request.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
        /// <param name="supplierDto">The updated supplier data.</param>
        /// <returns>HTTP 200 OK with the updated supplier data, or HTTP 404 Not Found if not found.</returns>
        [HttpPut("{supplierId}")]
        public async Task<IActionResult> UpdateSupplier(int supplierId, [FromBody] SupplierDto supplierDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

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
            return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Restores a deleted supplier by its ID.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier to restore.</param>
        /// <returns>HTTP 200 OK if successful, or HTTP 404 Not Found if the supplier does not exist.</returns>
        [HttpPut("{supplierId}/undelete")]
        public async Task<IActionResult> UndeleteSupplier(int supplierId)
        {
            var result = await _supplierService.UndeleteSupplierAsync(supplierId);
            return result.IsSuccess ? Ok(new { Message = "Supplier successfully restored." }) : NotFound(result.ErrorMessage);
        }
       
        /// <summary>
        /// Adds an item to a supplier's inventory.
        /// </summary>
        /// <param name="supplierId">The ID of the supplier to which the item will be added.</param>
        /// <param name="dto">The data transfer object containing the SKU and quantity of the item to add.</param>
        /// <returns>
        /// HTTP 200 OK with a success message if the item is added successfully, 
        /// or HTTP 400 Bad Request if the input data is invalid or the operation fails.
        /// </returns>
        [HttpPost("{supplierId}/items")]
        public async Task<IActionResult> AddItemToSupplier(int supplierId, [FromBody] AddItemToSupplierDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _supplierService.AddItemToSupplierAsync(supplierId, dto.Sku);
            return result.IsSuccess ? Ok(new { Message = "Item successfully added to supplier." }) : BadRequest(result.ErrorMessage);
        }
    }
}
