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
                    CustomerType = DetermineCustomerType(customer),
                    CustomerName = DetermineCustomerName(customer),
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

                // Según el tipo, guardar en la tabla correspondiente
                switch (customer)
                {
                    case BusinessCustomer businessCustomer:
                        await CreateBusinessCustomerAsync(customerEntity.CustomerId, businessCustomer);
                        break;
                    case IndividualCustomer individualCustomer:
                        await CreateIndividualCustomerAsync(customerEntity.CustomerId, individualCustomer);
                        break;
                    case PremiumCustomer premiumCustomer:
                        await CreatePremiumCustomerAsync(customerEntity.CustomerId, premiumCustomer);
                        break;
                }

                // Asignar el ID generado al modelo
                customer.Id = customerEntity.CustomerId.ToString();

                return Result<Customer>.Success(customer);
            }
            catch (Exception ex)
            {
                return Result<Customer>.Failure($"Error al crear el cliente: {ex.Message}");
            }
        }

        private string DetermineCustomerType(Customer customer)
        {
            return customer switch
            {
                BusinessCustomer => "Business",
                IndividualCustomer => "Individual",
                PremiumCustomer => "Premium",
                _ => "Unknown"
            };
        }

        private string DetermineCustomerName(Customer customer)
        {
            return customer switch
            {
                BusinessCustomer business => business.CompanyName,
                _ => $"{customer.FirstName} {customer.LastName}"
            };
        }

        private async Task CreateBusinessCustomerAsync(int customerId, BusinessCustomer businessCustomer)
        {
            var businessEntity = new BusinessCustomerEntity
            {
                CustomerId = customerId,
                CompanyName = businessCustomer.CompanyName,
                TaxId = businessCustomer.TaxId,
                Industry = businessCustomer.Industry,
                EmployeeCount = businessCustomer.EmployeeCount,
                AnnualRevenue = businessCustomer.AnnualRevenue,
                BusinessSize = businessCustomer.BusinessSize,
                VolumeDiscountRate = businessCustomer.VolumeDiscountRate,
                CreditTerms = businessCustomer.CreditTerms
            };

            _context.BusinessCustomerEntities.Add(businessEntity);
            await _context.SaveChangesAsync();
        }

        private async Task CreateIndividualCustomerAsync(int customerId, IndividualCustomer individualCustomer)
        {
            var individualEntity = new IndividualCustomerEntity
            {
                CustomerId = customerId,
                IsEligibleForPromotions = individualCustomer.IsEligibleForPromotions,
                CommunicationPreference = individualCustomer.CommunicationPreference,
                LoyaltyPoints = individualCustomer.LoyaltyPoints,
                LastPurchaseDate = individualCustomer.LastPurchaseDate
            };

            _context.IndividualCustomerEntities.Add(individualEntity);
            await _context.SaveChangesAsync();
        }

        private async Task CreatePremiumCustomerAsync(int customerId, PremiumCustomer premiumCustomer)
        {
            var premiumEntity = new PremiumCustomerEntity
            {
                CustomerId = customerId,
                DiscountRate = premiumCustomer.DiscountRate,
                MembershipStartDate = premiumCustomer.MembershipStartDate,
                MembershipExpiryDate = premiumCustomer.MembershipExpiryDate,
                TierLevel = premiumCustomer.TierLevel
            };

            _context.PremiumCustomerEntities.Add(premiumEntity);
            await _context.SaveChangesAsync();
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

                // Según el tipo, cargar los datos específicos
                Customer customer = customerEntity.CustomerType switch
                {
                    "Business" => await GetBusinessCustomerAsync(customerEntity),
                    "Individual" => await GetIndividualCustomerAsync(customerEntity),
                    "Premium" => await GetPremiumCustomerAsync(customerEntity),
                    _ => MapToBaseCustomer(customerEntity)
                };

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

        private async Task<BusinessCustomer> GetBusinessCustomerAsync(CustomerEntity baseEntity)
        {
            var businessEntity = await _context.BusinessCustomerEntities
                .FirstOrDefaultAsync(b => b.CustomerId == baseEntity.CustomerId);

            if (businessEntity == null)
            {
                return new BusinessCustomer();
            }

            var businessCustomer = new BusinessCustomer
            {
                Id = baseEntity.CustomerId.ToString(),
                FirstName = baseEntity.FirstName ?? string.Empty,
                LastName = baseEntity.LastName ?? string.Empty,
                BirthDate = baseEntity.BirthDate ?? DateOnly.MinValue,
                Email = baseEntity.Email ?? string.Empty,
                PhoneNumber = baseEntity.PhoneNumber ?? string.Empty,
                Address = baseEntity.Address ?? string.Empty,
                City = baseEntity.City ?? string.Empty,
                State = baseEntity.State ?? string.Empty,
                ZipCode = baseEntity.ZipCode ?? string.Empty,
                RegistrationDate = baseEntity.RegistrationDate,
                // Propiedades específicas
                CompanyName = businessEntity.CompanyName,
                TaxId = businessEntity.TaxId,
                Industry = businessEntity.Industry,
                EmployeeCount = businessEntity.EmployeeCount,
                AnnualRevenue = businessEntity.AnnualRevenue,
                BusinessSize = businessEntity.BusinessSize,
                VolumeDiscountRate = businessEntity.VolumeDiscountRate,
                CreditTerms = businessEntity.CreditTerms
            };

            return businessCustomer;
        }

        private async Task<IndividualCustomer> GetIndividualCustomerAsync(CustomerEntity baseEntity)
        {
            var individualEntity = await _context.IndividualCustomerEntities
                .FirstOrDefaultAsync(i => i.CustomerId == baseEntity.CustomerId);

            if (individualEntity == null)
            {
                return new IndividualCustomer();
            }

            var individualCustomer = new IndividualCustomer
            {
                Id = baseEntity.CustomerId.ToString(),
                FirstName = baseEntity.FirstName ?? string.Empty,
                LastName = baseEntity.LastName ?? string.Empty,
                BirthDate = baseEntity.BirthDate ?? DateOnly.MinValue,
                Email = baseEntity.Email ?? string.Empty,
                PhoneNumber = baseEntity.PhoneNumber ?? string.Empty,
                Address = baseEntity.Address ?? string.Empty,
                City = baseEntity.City ?? string.Empty,
                State = baseEntity.State ?? string.Empty,
                ZipCode = baseEntity.ZipCode ?? string.Empty,
                RegistrationDate = baseEntity.RegistrationDate,
                // Propiedades específicas
                IsEligibleForPromotions = individualEntity.IsEligibleForPromotions,
                CommunicationPreference = individualEntity.CommunicationPreference,
                LoyaltyPoints = individualEntity.LoyaltyPoints,
                LastPurchaseDate = individualEntity.LastPurchaseDate
            };

            return individualCustomer;
        }

        private async Task<PremiumCustomer> GetPremiumCustomerAsync(CustomerEntity baseEntity)
        {
            var premiumEntity = await _context.PremiumCustomerEntities
                .FirstOrDefaultAsync(p => p.CustomerId == baseEntity.CustomerId);

            if (premiumEntity == null)
            {
                return new PremiumCustomer();
            }

            var premiumCustomer = new PremiumCustomer
            {
                Id = baseEntity.CustomerId.ToString(),
                FirstName = baseEntity.FirstName ?? string.Empty,
                LastName = baseEntity.LastName ?? string.Empty,
                BirthDate = baseEntity.BirthDate ?? DateOnly.MinValue,
                Email = baseEntity.Email ?? string.Empty,
                PhoneNumber = baseEntity.PhoneNumber ?? string.Empty,
                Address = baseEntity.Address ?? string.Empty,
                City = baseEntity.City ?? string.Empty,
                State = baseEntity.State ?? string.Empty,
                ZipCode = baseEntity.ZipCode ?? string.Empty,
                RegistrationDate = baseEntity.RegistrationDate,
                // Propiedades específicas
                DiscountRate = premiumEntity.DiscountRate,
                MembershipStartDate = premiumEntity.MembershipStartDate,
                MembershipExpiryDate = premiumEntity.MembershipExpiryDate,
                TierLevel = premiumEntity.TierLevel
            };

            return premiumCustomer;
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