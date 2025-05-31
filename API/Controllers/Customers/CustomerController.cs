using API.Models;
using API.Models.Customers;
using softserve.projectlabs.Shared.DTOs.Customer;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.DTOs.Category;

namespace API.Controllers
{
    /// <summary>
    /// API Controller for managing Customer operations.
    /// </summary>
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        /// <summary>
        /// Constructor with dependency injection for ICustomerService.
        /// </summary>
        /// <param name="customerService">The customer service instance</param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="dto">Customer data transfer object containing creation information</param>
        /// <returns>HTTP response with the created customer or error message</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerCreateDto dto)
        {
            var result = await _customerService.AddCustomerAsync(dto);
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves a customer by its unique ID.
        /// </summary>
        /// <param name="customerId">Unique identifier of the customer</param>
        /// <returns>HTTP response with the customer or error message</returns>
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var result = await _customerService.GetCustomerByIdAsync(customerId);
            return result.IsSuccess
                ? Ok(result.Data)
                : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Retrieves all customers.
        /// </summary>
        /// <returns>HTTP response with the list of customers or error message</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await _customerService.GetAllCustomersAsync();
            return result.IsSuccess
                ? Ok(result.Data)
                : BadRequest(result.ErrorMessage);
        }
    }
}