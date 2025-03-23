﻿using API.Models.Logistics;
using API.Services.Logistics;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.Logistics
{
    [Route("api/orders")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrderController"/> class.
        /// </summary>
        /// <param name="orderService">The order service.</param>
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="order">The order to create.</param>
        /// <returns>The created order.</returns>
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            var result = await _orderService.CreateOrderAsync(order);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Gets an order by its identifier.
        /// </summary>
        /// <param name="id">The order identifier.</param>
        /// <returns>The order with the specified identifier.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Gets all orders.
        /// </summary>
        /// <returns>All orders.</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var result = await _orderService.GetAllOrdersAsync();
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Updates an existing order.
        /// </summary>
        /// <param name="order">The order to update.</param>
        /// <returns>The updated order.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] Order order)
        {
            var result = await _orderService.UpdateOrderAsync(order);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Deletes an order by its identifier.
        /// </summary>
        /// <param name="id">The order identifier.</param>
        /// <returns>The result of the deletion.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var result = await _orderService.DeleteOrderAsync(id);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }
    }
}
