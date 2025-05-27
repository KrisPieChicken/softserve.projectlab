using Microsoft.AspNetCore.Mvc;
using softserve.projectlabs.Shared.Interfaces;
using AutoMapper;
using API.Models.Logistics.Order;
using softserve.projectlabs.Shared.DTOs.Order;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers.Logistics
{
    /// <summary>
    /// Controller for managing orders in the logistics system.
    /// Provides endpoints for retrieving, updating, deleting, and fulfilling orders.
    /// </summary>
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The service for handling order operations.</param>
        /// <param name="mapper">The mapper for DTO conversions.</param>
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        /// <summary>
        /// Retrieves an order by its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the order to retrieve.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the order details if found, 
        /// or a 404 Not Found response if the order does not exist.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all orders in the system.
        /// </summary>
        /// <returns>
        /// An <see cref="IActionResult"/> containing a list of all orders if any exist, 
        /// or a 404 Not Found response if no orders are found.
        /// </returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing order by its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the order to update.</param>
        /// <param name="order">The updated order details.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> containing the updated order details if successful, 
        /// or a 404 Not Found response if the order does not exist.
        /// </returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order order)
        {
            var orderDto = _mapper.Map<OrderDto>(order);
            orderDto.OrderId = id;
            var result = await _orderService.UpdateOrderAsync(orderDto);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Deletes an order by its unique ID.
        /// </summary>
        /// <param name="id">The unique identifier of the order to delete.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the operation:
        /// - 204 No Content if the order was successfully deleted.
        /// - 404 Not Found if the order does not exist.
        /// </returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            if (result.IsNoContent)
            {
                return NoContent();
            }
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves an order by its associated cart ID.
        /// </summary>
        /// <param name="cartId">The ID of the cart associated with the order.</param>
        /// <returns>An <see cref="IActionResult"/> containing the order details or an error message.</returns>
        [HttpGet("cart/{cartId}")]
        [Authorize]
        public async Task<IActionResult> RetrieveOrderByCartId(int cartId)
        {
            var result = await _orderService.RetrieveOrderByCartIdAsync(cartId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Marks an order as fulfilled by its unique ID.
        /// </summary>
        /// <param name="orderId">The unique identifier of the order to fulfill.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> indicating the result of the operation:
        /// - 200 OK with a success message if the order was fulfilled.
        /// - 400 Bad Request if the operation failed.
        /// </returns>
        [HttpPost("{orderId}/fulfill")]
        [Authorize]
        public async Task<IActionResult> FulfillOrder(int orderId)
        {
            var result = await _orderService.FulfillOrderAsync(orderId);
            return result.IsSuccess ? Ok("Order fulfilled successfully.") : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all unsaved orders and saves them to the database.
        /// </summary>
        /// <returns>An <see cref="IActionResult"/> indicating the result of the operation.</returns>
        [HttpPost("save-unsaved-orders")]
        [Authorize]
        public async Task<IActionResult> RetrieveAndSaveAllUnsavedOrders()
        {
            var result = await _orderService.RetrieveAndSaveAllUnsavedOrdersAsync();
            return result.IsSuccess ? Ok("All unsaved orders have been saved.") : BadRequest(result.ErrorMessage);
        }
    }
}
