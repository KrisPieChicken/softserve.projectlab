using Microsoft.AspNetCore.Mvc;
using API.Models.IntAdmin;
using AutoMapper;
using API.Models;
using softserve.projectlabs.Shared.DTOs;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Controllers
{

    /// <summary>
    /// Controller for managing warehouse operations.
    /// </summary>
    [ApiController]
    [Route("api/Warehouse")]
    public class WarehouseController : ControllerBase
    {
        private readonly IWarehouseService _warehouseService;
        private readonly IMapper _mapper;
        private readonly ILogger<WarehouseController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="WarehouseController"/> class.
        /// </summary>
        /// <param name="warehouseService">Service for warehouse operations.</param>
        /// <param name="mapper">Mapper for DTO conversions.</param>
        /// <param name="logger">Logger for logging operations.</param>
        public WarehouseController(IWarehouseService warehouseService, IMapper mapper, ILogger<WarehouseController> logger)
        {
            _warehouseService = warehouseService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a warehouse by its ID.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse.</param>
        /// <returns>The warehouse details if found, otherwise a NotFound result.</returns>
        [HttpGet("{warehouseId}")]
        public async Task<IActionResult> GetWarehouseById(int warehouseId)
        {
            var result = await _warehouseService.GetWarehouseByIdAsync(warehouseId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all warehouses.
        /// </summary>
        /// <returns>A list of all warehouses.</returns>
        [HttpGet("")]
        [ProducesResponseType(typeof(List<WarehouseResponseDto>), 200)]
        public async Task<ActionResult<List<WarehouseResponseDto>>> GetAllWarehouses()
        {
            var warehouses = await _warehouseService.GetWarehousesAsync();

            var result = warehouses.Select(w => new WarehouseResponseDto
            {
                WarehouseId = w.WarehouseId,
                Name = w.Name,
                Location = w.Location,
                Capacity = w.Capacity,
                Items = w.Items.Select(i => new ItemDto
                {
                    Sku = i.Sku,
                    ItemName = i.ItemName,
                    ItemDescription = i.ItemDescription,
                    ItemPrice = i.ItemPrice,
                    CurrentStock = i.CurrentStock
                }).ToList(),
                BranchId = w.BranchId
            }).ToList();

            return Ok(result);
        }

        /// <summary>
        /// Adds an item to a warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse.</param>
        /// <param name="itemDto">The item details to add.</param>
        /// <returns>A success or failure result.</returns>
        [HttpPost("{warehouseId}/items")]
        public async Task<IActionResult> AddItem(int warehouseId, [FromBody] AddItemToWarehouseDTO itemDto)
        {
            _logger.LogInformation("Starting AddItem for WarehouseId: {WarehouseId}", warehouseId);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for WarehouseId: {WarehouseId}", warehouseId);
                return BadRequest(ModelState);
            }

            var result = await _warehouseService.AddItemToWarehouseAsync(warehouseId, itemDto);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully linked item to WarehouseId: {WarehouseId}, SKU: {Sku}", warehouseId, itemDto.Sku);
                return Ok(new { Message = "Item successfully linked to the warehouse." });
            }
            else
            {
                _logger.LogError("Failed to link item to WarehouseId: {WarehouseId}, SKU: {Sku}. Error: {ErrorMessage}", warehouseId, itemDto.Sku, result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
        }

        /// <summary>
        /// Removes an item from a warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse.</param>
        /// <param name="itemId">The ID of the item to remove.</param>
        /// <returns>A success or failure result.</returns>
        [HttpDelete("{warehouseId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItem(int warehouseId, int itemId)
        {
            var result = await _warehouseService.RemoveItemFromWarehouseAsync(warehouseId, itemId);

            if (result.IsNoContent)
                return NoContent();

            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Checks the stock of an item in a warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse.</param>
        /// <param name="itemId">The ID of the item.</param>
        /// <returns>The stock level of the item if found, otherwise a NotFound result.</returns>
        [HttpGet("{warehouseId}/items/{itemId}/stock")]
        public async Task<IActionResult> CheckStock(int warehouseId, int itemId)
        {
            var result = await _warehouseService.CheckWarehouseStockAsync(warehouseId, itemId);
            return result.IsSuccess
                ? Ok(new { ItemId = itemId, Stock = result.Data })
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Transfers an item between warehouses.
        /// </summary>
        /// <param name="transferRequest">The transfer request details.</param>
        /// <returns>A success or failure result.</returns>
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferItem([FromBody] TransferRequestDto transferRequest)
        {
            var result = await _warehouseService.TransferItemAsync(
                transferRequest.SourceWarehouseId,
                transferRequest.ItemId,
                transferRequest.Quantity,
                transferRequest.TargetWarehouseId);

            return result.IsSuccess
                ? Ok(new { Message = "Transfer completed successfully" })
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves items with low stock in a warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse.</param>
        /// <param name="threshold">The stock threshold to consider as low. Default is 5.</param>
        /// <returns>A list of low stock items if found, otherwise a NotFound result.</returns>
        [HttpGet("{warehouseId}/low-stock")]
        public async Task<IActionResult> GetLowStockItems(int warehouseId, [FromQuery] int threshold = 5)
        {
            var result = await _warehouseService.GetLowStockItemsAsync(warehouseId, threshold);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Calculates the total inventory value of a warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse.</param>
        /// <returns>The total inventory value if successful, otherwise a BadRequest result.</returns>
        [HttpGet("{warehouseId}/inventory-value")]
        public async Task<IActionResult> GetTotalInventoryValue(int warehouseId)
        {
            var result = await _warehouseService.CalculateTotalInventoryValueAsync(warehouseId);
            return result.IsSuccess
                ? Ok(new { TotalValue = result.Data })
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Generates an inventory report for a warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse.</param>
        /// <returns>The inventory report if successful, otherwise a BadRequest result.</returns>
        [HttpGet("{warehouseId}/inventory-report")]
        public async Task<IActionResult> GetInventoryReport(int warehouseId)
        {
            var result = await _warehouseService.GenerateInventoryReportAsync(warehouseId);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Deletes a warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse to delete.</param>
        /// <returns>A success or failure result.</returns>
        [HttpDelete("{warehouseId}")]
        public async Task<IActionResult> DeleteWarehouse(int warehouseId)
        {
            _logger.LogInformation("Deleting WarehouseId: {WarehouseId}", warehouseId);

            var result = await _warehouseService.DeleteWarehouseAsync(warehouseId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully deleted WarehouseId: {WarehouseId}", warehouseId);
                return Ok(new { Message = "Warehouse successfully deleted." });
            }
            else
            {
                _logger.LogError("Failed to delete WarehouseId: {WarehouseId}. Error: {ErrorMessage}", warehouseId, result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
        }

        /// <summary>
        /// Restores a deleted warehouse.
        /// </summary>
        /// <param name="warehouseId">The ID of the warehouse to restore.</param>
        /// <returns>A success or failure result.</returns>
        [HttpPost("{warehouseId}/undelete")]
        public async Task<IActionResult> UndeleteWarehouse(int warehouseId)
        {
            _logger.LogInformation("Restoring WarehouseId: {WarehouseId}", warehouseId);

            var result = await _warehouseService.UndeleteWarehouseAsync(warehouseId);

            if (result.IsSuccess)
            {
                _logger.LogInformation("Successfully restored WarehouseId: {WarehouseId}", warehouseId);
                return Ok(new { Message = "Warehouse successfully restored." });
            }
            else
            {
                _logger.LogError("Failed to restore WarehouseId: {WarehouseId}. Error: {ErrorMessage}", warehouseId, result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
        }
    }

    /// <summary>
    /// DTO for transferring items between warehouses.
    /// </summary>
    public class TransferRequestDto
    {
        /// <summary>
        /// The ID of the source warehouse.
        /// </summary>
        public int SourceWarehouseId { get; set; }

        /// <summary>
        /// The ID of the target warehouse.
        /// </summary>
        public int TargetWarehouseId { get; set; }

        /// <summary>
        /// The ID of the item to transfer.
        /// </summary>
        public int ItemId { get; set; }

        /// <summary>
        /// The quantity of the item to transfer.
        /// </summary>
        public int Quantity { get; set; }
    }
}