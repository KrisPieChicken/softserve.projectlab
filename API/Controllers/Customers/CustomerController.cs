using API.Models;
using API.Models.Customers;
using API.Models.DTOs;
using API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Crea un nuevo cliente.
        /// </summary>
        /// <param name="customerDto">Datos del cliente</param>
        /// <returns>El cliente creado</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCustomer([FromBody] CustomerDto customerDto)
        {
            // Convertir el DTO al tipo de cliente correcto
            Customer customer = ConvertToCustomer(customerDto);
            
            var result = await _customerService.AddCustomerAsync(customer);
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        /// <summary>
        /// Obtiene un cliente por su ID.
        /// </summary>
        /// <param name="customerId">ID del cliente</param>
        /// <returns>El cliente encontrado</returns>
        [HttpGet("{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId)
        {
            var result = await _customerService.GetCustomerByIdAsync(customerId);
            return result.IsSuccess ? Ok(result.Data) : NotFound(result.ErrorMessage);
        }

        /// <summary>
        /// Obtiene todos los clientes.
        /// </summary>
        /// <returns>Lista de clientes</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllCustomers()
        {
            var result = await _customerService.GetAllCustomersAsync();
            return result.IsSuccess ? Ok(result.Data) : BadRequest(result.ErrorMessage);
        }

        private Customer ConvertToCustomer(CustomerDto dto)
        {
            return dto.CustomerType.ToLower() switch
            {
                "business" => new BusinessCustomer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode,
                    // Propiedades específicas de BusinessCustomer
                    CompanyName = dto.CompanyName,
                    TaxId = dto.TaxId,
                    Industry = dto.Industry,
                    EmployeeCount = dto.EmployeeCount,
                    AnnualRevenue = dto.AnnualRevenue,
                    BusinessSize = dto.BusinessSize,
                    VolumeDiscountRate = dto.VolumeDiscountRate,
                    CreditTerms = dto.CreditTerms
                },
                "individual" => new IndividualCustomer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode,
                    // Propiedades específicas de IndividualCustomer
                    IsEligibleForPromotions = dto.IsEligibleForPromotions,
                    CommunicationPreference = dto.CommunicationPreference,
                    LoyaltyPoints = dto.LoyaltyPoints,
                    LastPurchaseDate = dto.LastPurchaseDate
                },
                "premium" => new PremiumCustomer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode,
                    // Propiedades específicas de PremiumCustomer
                    DiscountRate = dto.DiscountRate,
                    MembershipStartDate = dto.MembershipStartDate,
                    MembershipExpiryDate = dto.MembershipExpiryDate,
                    TierLevel = dto.TierLevel
                },
                _ => new Customer
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    BirthDate = dto.BirthDate,
                    Email = dto.Email,
                    PhoneNumber = dto.PhoneNumber,
                    Address = dto.Address,
                    City = dto.City,
                    State = dto.State,
                    ZipCode = dto.ZipCode
                }
            };
        }
    }
}