using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Services.Logistics;
using API.Models.Logistics.Interfaces;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs;
using AutoMapper; // Added for AutoMapper

namespace API.Controllers
{
    /// <summary>
    /// Controller for managing supplier orders.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierOrderController : ControllerBase
    {
        private readonly ISupplierOrderService _service;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="SupplierOrderController"/> class.
        /// </summary>
        /// <param name="service">The supplier order service.</param>
        /// <param name="mapper">The AutoMapper instance for DTO mapping.</param>
        public SupplierOrderController(ISupplierOrderService service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves all supplier orders.
        /// </summary>
        /// <returns>A list of supplier orders.</returns>
        [HttpGet]
        public async Task<ActionResult<List<ISupplierOrder>>> GetAllOrders()
        {
            var supplierOrderDtos = await _service.GetAllSupplierOrdersAsync();
            var supplierOrders = _mapper.Map<List<ISupplierOrder>>(supplierOrderDtos);
            return Ok(supplierOrders);
        }

        /// <summary>
        /// Retrieves a supplier order by its ID.
        /// </summary>
        /// <param name="id">The ID of the supplier order.</param>
        /// <returns>The supplier order with the specified ID.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ISupplierOrder>> GetOrderById(int id)
        {
            var supplierOrderDto = await _service.GetSupplierOrderByIdAsync(id);
            if (supplierOrderDto == null) return NotFound();
            var supplierOrder = _mapper.Map<ISupplierOrder>(supplierOrderDto);
            return Ok(supplierOrder);
        }

        /// <summary>
        /// Creates a new supplier order.
        /// </summary>
        /// <param name="order">The supplier order to create.</param>
        /// <returns>The created supplier order.</returns>
        [HttpPost]
        public async Task<ActionResult<ISupplierOrder>> CreateOrder([FromBody] ISupplierOrder order)
        {
            var supplierOrderDto = _mapper.Map<SupplierOrderDto>(order);
            var newOrderDto = await _service.AddSupplierOrderAsync(supplierOrderDto);
            var newOrder = _mapper.Map<ISupplierOrder>(newOrderDto);
            return CreatedAtAction(nameof(GetOrderById), new { id = newOrder.OrderId }, newOrder);
        }

        /// <summary>
        /// Updates an existing supplier order.
        /// </summary>
        /// <param name="id">The ID of the supplier order to update.</param>
        /// <param name="order">The updated supplier order data.</param>
        /// <returns>No content if the update is successful, or a not found result if the order does not exist.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] ISupplierOrder order)
        {
            if (id != order.OrderId) return BadRequest();
            var supplierOrderDto = _mapper.Map<SupplierOrderDto>(order);
            var updated = await _service.UpdateSupplierOrderAsync(supplierOrderDto);
            if (!updated) return NotFound();
            return NoContent();
        }

        /// <summary>
        /// Deletes a supplier order by its ID.
        /// </summary>
        /// <param name="id">The ID of the supplier order to delete.</param>
        /// <returns>No content if the deletion is successful, or a not found result if the order does not exist.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var deleted = await _service.DeleteSupplierOrderAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
