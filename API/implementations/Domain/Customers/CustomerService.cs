using API.Data;
using API.Models.Customers;
using API.Services.Customers;
using Microsoft.EntityFrameworkCore;

namespace API.Implementations.Domain.Customers;

/// <summary>
/// Implementation of the ICustomerService interface using Entity Framework.
/// </summary>
public class CustomerService : ICustomerService
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of the CustomerService class.
    /// </summary>
    public CustomerService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all customers.
    /// </summary>
    /// <returns>A collection of all customers.</returns>
    public async Task<IEnumerable<Customer>> GetAllCustomersAsync()
    {
        return await _context.Customers.ToListAsync();
    }

    /// <summary>
    /// Gets a customer by ID.
    /// </summary>
    /// <param name="id">The ID of the customer to retrieve.</param>
    /// <returns>The customer if found; otherwise, null.</returns>
    public async Task<Customer?> GetCustomerByIdAsync(string id)
    {
        return await _context.Customers
            .Include(c => c.LineOfCredit)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <summary>
    /// Gets customers by type.
    /// </summary>
    /// <param name="customerType">The type of customers to retrieve (Premium, Business, Individual).</param>
    /// <returns>A collection of customers of the specified type.</returns>
    public async Task<IEnumerable<Customer>> GetCustomersByTypeAsync(string customerType)
    {
        return customerType.ToLower() switch
        {
            "premium" => await _context.Customers.OfType<PremiumCustomer>().ToListAsync(),
            "business" => await _context.Customers.OfType<BusinessCustomer>().ToListAsync(),
            "individual" => await _context.Customers.OfType<IndividualCustomer>().ToListAsync(),
            _ => Enumerable.Empty<Customer>()
        };
    }

    /// <summary>
    /// Creates a new customer.
    /// </summary>
    /// <param name="customer">The customer to create.</param>
    /// <returns>The created customer.</returns>
    public async Task<Customer> CreateCustomerAsync(Customer customer)
    {
        // Ensure the customer has an ID
        if (string.IsNullOrEmpty(customer.Id))
        {
            customer.Id = Guid.NewGuid().ToString();
        }
        
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return customer;
    }

    /// <summary>
    /// Updates an existing customer.
    /// </summary>
    /// <param name="id">The ID of the customer to update.</param>
    /// <param name="customer">The updated customer information.</param>
    /// <returns>The updated customer if found; otherwise, null.</returns>
    public async Task<Customer?> UpdateCustomerAsync(string id, Customer customer)
    {
        var existingCustomer = await _context.Customers.FindAsync(id);
        if (existingCustomer == null)
        {
            return null;
        }
        
        // Update properties depending on the type of customer
        if (existingCustomer is BusinessCustomer existingBusiness && customer is BusinessCustomer updatedBusiness)
        {
            _context.Entry(existingBusiness).CurrentValues.SetValues(updatedBusiness);
        }
        else if (existingCustomer is IndividualCustomer existingIndividual && customer is IndividualCustomer updatedIndividual)
        {
            _context.Entry(existingIndividual).CurrentValues.SetValues(updatedIndividual);
        }
        else if (existingCustomer is PremiumCustomer existingPremium && customer is PremiumCustomer updatedPremium)
        {
            _context.Entry(existingPremium).CurrentValues.SetValues(updatedPremium);
        }
        else
        {
            _context.Entry(existingCustomer).CurrentValues.SetValues(customer);
        }

        await _context.SaveChangesAsync();
        return existingCustomer;
    }

    /// <summary>
    /// Deletes a customer.
    /// </summary>
    /// <param name="id">The ID of the customer to delete.</param>
    /// <returns>True if the customer was deleted; otherwise, false.</returns>
    public async Task<bool> DeleteCustomerAsync(string id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null)
        {
            return false;
        }
        
        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Creates a line of credit for a customer.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="lineOfCredit">The line of credit to create.</param>
    /// <returns>The created line of credit.</returns>
    public async Task<LineOfCredit> CreateLineOfCreditAsync(string customerId, LineOfCredit lineOfCredit)
    {
        var customer = await _context.Customers.FindAsync(customerId);
        if (customer == null)
        {
            throw new KeyNotFoundException($"Customer with ID {customerId} not found.");
        }
        
        lineOfCredit.CustomerId = customerId;
        _context.LineOfCredits.Add(lineOfCredit);
        await _context.SaveChangesAsync();
        
        return lineOfCredit;
    }

    /// <summary>
    /// Gets a customer's line of credit.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>The customer's line of credit if found; otherwise, null.</returns>
    public async Task<LineOfCredit?> GetLineOfCreditAsync(string customerId)
    {
        var customer = await _context.Customers
            .Include(c => c.LineOfCredit)
            .FirstOrDefaultAsync(c => c.Id == customerId);
            
        return customer?.LineOfCredit;
    }

    /// <summary>
    /// Updates a customer's line of credit.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="lineOfCredit">The updated line of credit information.</param>
    /// <returns>The updated line of credit if found; otherwise, null.</returns>
    public async Task<LineOfCredit?> UpdateLineOfCreditAsync(string customerId, LineOfCredit lineOfCredit)
    {
        var existingLineOfCredit = await _context.LineOfCredits
            .FirstOrDefaultAsync(loc => loc.CustomerId == customerId);
            
        if (existingLineOfCredit == null)
        {
            return null;
        }
        
        // Preserve the ID and CustomerId
        lineOfCredit.Id = existingLineOfCredit.Id;
        lineOfCredit.CustomerId = customerId;
        
        // Update the line of credit
        _context.Entry(existingLineOfCredit).CurrentValues.SetValues(lineOfCredit);
        await _context.SaveChangesAsync();
        
        return existingLineOfCredit;
    }

    /// <summary>
    /// Searches for customers based on search criteria.
    /// </summary>
    /// <param name="searchTerm">The search term to match against customer properties.</param>
    /// <returns>A collection of customers matching the search criteria.</returns>
    public async Task<IEnumerable<Customer>> SearchCustomersAsync(string searchTerm)
    {
        searchTerm = searchTerm.ToLower();
        
        return await _context.Customers
            .Where(c => c.FirstName.ToLower().Contains(searchTerm) || 
                c.LastName.ToLower().Contains(searchTerm) || 
                c.Email.ToLower().Contains(searchTerm) || 
                c.PhoneNumber.ToLower().Contains(searchTerm) ||
                (c.FirstName + " " + c.LastName).ToLower().Contains(searchTerm))
            .ToListAsync();
    }

    /// <summary>
    /// Gets the transaction history for a customer's line of credit.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <returns>A collection of credit transactions if found; otherwise, null.</returns>
    public async Task<IEnumerable<CreditTransaction>?> GetCreditTransactionHistoryAsync(string customerId)
    {
        var lineOfCredit = await _context.LineOfCredits
            .Include(loc => loc.Transactions)
            .FirstOrDefaultAsync(loc => loc.CustomerId == customerId);
            
        return lineOfCredit?.Transactions;
    }

    /// <summary>
    /// Makes a payment on a customer's line of credit.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="amount">The payment amount.</param>
    /// <param name="description">Optional description of the payment.</param>
    /// <returns>The updated line of credit if found; otherwise, null.</returns>
    public async Task<LineOfCredit?> MakePaymentAsync(string customerId, decimal amount, string? description = null)
    {
        var lineOfCredit = await _context.LineOfCredits
            .Include(loc => loc.Transactions)
            .FirstOrDefaultAsync(loc => loc.CustomerId == customerId);
            
        if (lineOfCredit == null)
        {
            return null;
        }
        
        // Crear nueva transacción
        var transaction = new CreditTransaction
        {
            TransactionType = "Payment",
            Amount = amount,
            Description = description ?? "Payment",
            TransactionDate = DateTime.UtcNow,
            LineOfCreditId = lineOfCredit.Id,
            LineOfCredit = lineOfCredit
        };
        
        lineOfCredit.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        return lineOfCredit;
    }

    /// <summary>
    /// Makes a charge on a customer's line of credit.
    /// </summary>
    /// <param name="customerId">The ID of the customer.</param>
    /// <param name="amount">The charge amount.</param>
    /// <param name="description">Optional description of the charge.</param>
    /// <returns>The updated line of credit if found; otherwise, null.</returns>
    public async Task<LineOfCredit?> MakeChargeAsync(string customerId, decimal amount, string? description = null)
    {
        var lineOfCredit = await _context.LineOfCredits
            .Include(loc => loc.Transactions)
            .FirstOrDefaultAsync(loc => loc.CustomerId == customerId);
            
        if (lineOfCredit == null)
        {
            return null;
        }
        
        // Verificar si hay suficiente crédito disponible
        decimal balance = await _context.CreditTransactions
            .Where(t => t.LineOfCreditId == lineOfCredit.Id)
            .SumAsync(t => t.TransactionType == "Payment" ? -t.Amount : t.Amount);
            
        if (lineOfCredit.CreditLimit - balance < amount)
        {
            throw new InvalidOperationException("Insufficient available credit");
        }
        
        // Crear nueva transacción
        var transaction = new CreditTransaction
        {
            TransactionType = "Charge",
            Amount = amount,
            Description = description ?? "Purchase",
            TransactionDate = DateTime.UtcNow,
            LineOfCreditId = lineOfCredit.Id,
            LineOfCredit = lineOfCredit
        };
        
        lineOfCredit.Transactions.Add(transaction);
        await _context.SaveChangesAsync();
        
        return lineOfCredit;
    }
}