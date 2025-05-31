using API.Data;
using API.Data.Entities;
using API.Models;
using API.Models.Customers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;
using AutoMapper;

namespace API.Implementations.Domain;

public class CustomerDomain
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public CustomerDomain(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
    {
        try
        {
            // Usar AutoMapper para mapear Customer a CustomerEntity
            var customerEntity = _mapper.Map<CustomerEntity>(customer);

            _context.CustomerEntities.Add(customerEntity);
            await _context.SaveChangesAsync();

            customer.CustomerId = customerEntity.CustomerId;

            return Result<Customer>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<Customer>.Failure($"Error al crear el cliente: {ex.Message}");
        }
    }

    public async Task<Result<Customer>> GetCustomerByIdAsync(int customerId)
    {
        try
        {
            var customerEntity = await _context.CustomerEntities
                .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if (customerEntity == null)
            {
                return Result<Customer>.Failure("Cliente no encontrado.");
            }

            // Usar AutoMapper para mapear CustomerEntity a Customer
            var customer = _mapper.Map<Customer>(customerEntity);

            return Result<Customer>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<Customer>.Failure($"Error al obtener el cliente: {ex.Message}");
        }
    }

    public async Task<Result<List<Customer>>> GetAllCustomersAsync()
    {
        try
        {
            var customerEntities = await _context.CustomerEntities.ToListAsync();
            var customers = _mapper.Map<List<Customer>>(customerEntities);

            return Result<List<Customer>>.Success(customers);
        }
        catch (Exception ex)
        {
            return Result<List<Customer>>.Failure($"Error al obtener los clientes: {ex.Message}");
        }
    }
}
