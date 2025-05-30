using API.Data;
using API.Data.Entities;
using API.Models;
using API.Models.Customers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using softserve.projectlabs.Shared.Utilities;

namespace API.Implementations.Domain
{
    public class CustomerDomain
    {
        private readonly ApplicationDbContext _context;

        public CustomerDomain(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Result<Customer>> CreateCustomerAsync(Customer customer)
        {
            try
            {
                // Crear la entidad Customer base
                var customerEntity = new CustomerEntity
                {
                    CustomerName = customer.FirstName + " " + customer.LastName,
                    CustomerContactNumber = customer.PhoneNumber,
                    CustomerContactEmail = customer.Email,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    BirthDate = customer.BirthDate,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    Address = customer.Address,
                    City = customer.City,
                    State = customer.State,
                    ZipCode = customer.ZipCode,
                    RegistrationDate = DateTime.UtcNow
                };

                // Guardar el Customer básico primero
                _context.CustomerEntities.Add(customerEntity);
                await _context.SaveChangesAsync();

                // Asignar el ID generado al modelo
                customer.Id = customerEntity.CustomerId.ToString();

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
                // Obtener los datos básicos del cliente
                var customerEntity = await _context.CustomerEntities
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (customerEntity == null)
                {
                    return Result<Customer>.Failure("Cliente no encontrado.");
                }

                // Mapear la entidad a modelo Customer
                var customer = MapToBaseCustomer(customerEntity);

                return Result<Customer>.Success(customer);
            }
            catch (Exception ex)
            {
                return Result<Customer>.Failure($"Error al obtener el cliente: {ex.Message}");
            }
        }

        private Customer MapToBaseCustomer(CustomerEntity entity)
        {
            return new Customer
            {
                Id = entity.CustomerId.ToString(),
                FirstName = entity.FirstName ?? string.Empty,
                LastName = entity.LastName ?? string.Empty,
                BirthDate = entity.BirthDate ?? DateOnly.MinValue,
                Email = entity.Email ?? string.Empty,
                PhoneNumber = entity.PhoneNumber ?? string.Empty,
                Address = entity.Address ?? string.Empty,
                City = entity.City ?? string.Empty,
                State = entity.State ?? string.Empty,
                ZipCode = entity.ZipCode ?? string.Empty,
                RegistrationDate = entity.RegistrationDate
            };
        }

        public async Task<Result<List<Customer>>> GetAllCustomersAsync()
        {
            try
            {
                var customers = new List<Customer>();

                // Obtener todos los clientes básicos
                var customerEntities = await _context.CustomerEntities.ToListAsync();

                // Para cada cliente, obtener sus detalles específicos
                foreach (var entity in customerEntities)
                {
                    var customerResult = await GetCustomerByIdAsync(entity.CustomerId);
                    if (customerResult.IsSuccess)
                    {
                        customers.Add(customerResult.Data);
                    }
                }

                return Result<List<Customer>>.Success(customers);
            }
            catch (Exception ex)
            {
                return Result<List<Customer>>.Failure($"Error al obtener los clientes: {ex.Message}");
            }
        }
    }
}