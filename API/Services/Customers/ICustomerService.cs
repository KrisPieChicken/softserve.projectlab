using API.Models;
using API.Models.Customers;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using softserve.projectlabs.Shared.Interfaces;
using softserve.projectlabs.Shared.DTOs.Customer;

namespace API.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<Result<Customer>> AddCustomerAsync(CustomerCreateDto customerDto);
        Task<Result<Customer>> GetCustomerByIdAsync(int customerId);
        Task<Result<List<Customer>>> GetAllCustomersAsync();
    }
}