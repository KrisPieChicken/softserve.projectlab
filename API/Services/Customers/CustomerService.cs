using API.Implementations.Domain;
using API.Models;
using API.Models.Customers;
using API.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs.Category;
using softserve.projectlabs.Shared.DTOs.Customer;
using AutoMapper;
using softserve.projectlabs.Shared.DTOs.Catalog;

namespace API.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDomain _customerDomain;
        private readonly IMapper _mapper;

        public CustomerService(CustomerDomain customerDomain, IMapper mapper)
        {
            _customerDomain = customerDomain;
            _mapper = mapper;
        }

        public async Task<Result<Customer>> AddCustomerAsync(CustomerCreateDto customerDto)
        {
            var domainModel = _mapper.Map<Customer>(customerDto);
            return await _customerDomain.CreateCustomerAsync(domainModel);
        }

        public async Task<Result<Customer>> GetCustomerByIdAsync(int customerId)
        {
            return await _customerDomain.GetCustomerByIdAsync(customerId);
        }

        public async Task<Result<List<Customer>>> GetAllCustomersAsync()
        {
            return await _customerDomain.GetAllCustomersAsync();
        }
    }
}