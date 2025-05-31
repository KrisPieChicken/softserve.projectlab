using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.DTOs.Warehouse;
using softserve.projectlabs.Shared.Interfaces;

namespace API.Controllers.Logistics
{
    /// <summary>
    /// Controller for managing warehouse operations.
    /// </summary>
    [Route("api/warehouses")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseController"/> class.
        /// </summary>
        /// <param name="warehouseService">Service for warehouse operations.</param>
        public WarehouseController(IWarehouseService warehouseService)
        {
            _warehouseService = warehouseService;
        }

        /// <summary>
        /// Retrieves all warehouses.
        /// </summary>
        /// <returns>A list of warehouses or a NotFound response if none exist.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var warehouses = await _warehouseService.GetAllWarehousesAsync();
            if (warehouses == null || !warehouses.Any())
                return NotFound("No warehouses found.");

            return Ok(warehouses);
        }

        /// <summary>
        /// Retrieves a warehouse by its ID.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <returns>The warehouse details or a NotFound response if not found.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetWarehouseById(int id)
        {
            //Here we are calling the service to get the warehouse by id
            var result = await _warehouseService.GetWarehouseByIdAsync(id);
            //if the result is successful, we return the data, otherwise we return a not found response
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Adds an item to a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <param name="addItemDto">The item details to add.</param>
        /// <returns>A success response or a BadRequest response if validation fails.</returns>
        [HttpPost("{id}/items")]
        [Authorize]
        public async Task<IActionResult> AddItemToWarehouse(int id, [FromBody] AddItemToWarehouseDto addItemDto)
        {
            if (addItemDto.WarehouseId != id)
                return BadRequest("Warehouse ID in the URL does not match the body.");

            var result = await _warehouseService.AddItemToWarehouseAsync(addItemDto.WarehouseId, addItemDto.Sku, addItemDto.CurrentStock);
            return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Removes an item from a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <param name="sku">The SKU of the item to remove.</param>
        /// <returns>A success response or a NotFound response if the item is not found.</returns>
        [HttpDelete("{id}/items/{sku}")]
        [Authorize]
        public async Task<IActionResult> RemoveItemFromWarehouse(int id, int sku)
        {
            var result = await _warehouseService.RemoveItemFromWarehouseAsync(id, sku);
            return result.IsSuccess ? Ok() : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Transfers an item between warehouses.
        /// </summary>
        /// <param name="sourceId">The ID of the source warehouse.</param>
        /// <param name="targetId">The ID of the target warehouse.</param>
        /// <param name="transferItemDto">The transfer details.</param>
        /// <returns>A success response or a BadRequest response if validation fails.</returns>
        [HttpPost("{sourceId}/items/{targetId}")]
        [Authorize]
        public async Task<IActionResult> TransferItem(int sourceId, int targetId, [FromBody] TransferItemDto transferItemDto)
        {
            if (transferItemDto.SourceWarehouseId != sourceId || transferItemDto.TargetWarehouseId != targetId)
                return BadRequest("Warehouse IDs in the URL do not match the body.");

            var result = await _warehouseService.TransferItemAsync(sourceId, transferItemDto.Sku, transferItemDto.Quantity, targetId);
            return result.IsSuccess ? Ok() : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves items with low stock in a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <param name="threshold">The stock threshold.</param>
        /// <returns>A list of low stock items or a NotFound response if none exist.</returns>
        [HttpGet("{id}/stock")]
        [Authorize]
        public async Task<IActionResult> GetLowStockItems(int id, [FromQuery] int threshold)
        {
            var result = await _warehouseService.GetLowStockItemsAsync(id, threshold);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Calculates the total inventory value of a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <returns>The total inventory value or a NotFound response if the warehouse is not found.</returns>
        [HttpGet("{id}/inventory")]
        [Authorize]
        public async Task<IActionResult> GetTotalInventoryValue(int id)
        {
            var result = await _warehouseService.CalculateTotalInventoryValueAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Generates an inventory report for a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <returns>The inventory report or a NotFound response if the warehouse is not found.</returns>
        [HttpGet("{id}/inventory-report")]
        [Authorize]
        public async Task<IActionResult> GenerateInventoryReport(int id)
        {
            var result = await _warehouseService.GenerateInventoryReportAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Soft deletes a warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <returns>A NoContent response if successful or a NotFound response if the warehouse is not found.</returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> SoftDeleteWarehouse(int id)
        {
            var result = await _warehouseService.SoftDeleteWarehouseAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Restores a soft-deleted warehouse.
        /// </summary>
        /// <param name="id">The ID of the warehouse.</param>
        /// <returns>A success response or a NotFound response if the warehouse is not found.</returns>
        [HttpPost("{id}/restore")]
        [Authorize]
        public async Task<IActionResult> UndeleteWarehouse(int id)
        {
            var result = await _warehouseService.UndeleteWarehouseAsync(id);
            return result.IsSuccess ? Ok() : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Creates a new warehouse.
        /// </summary>
        /// <param name="warehouseDto">The warehouse details.</param>
        /// <returns>The created warehouse or a BadRequest response if creation fails.</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateWarehouse([FromBody] WarehouseDto warehouseDto)
        {
            var result = await _warehouseService.CreateWarehouseAsync(warehouseDto);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

    }
}

