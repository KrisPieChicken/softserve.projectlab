namespace API.Models.Customers;

/// <summary>
/// Represents a customer with personal and financial information.
/// </summary>
public class Customer
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    
    public DateOnly BirthDate { get; set; }
    
    public string Email { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public string City { get; set; } = string.Empty;
    
    public string State { get; set; } = string.Empty;
    
    public string ZipCode { get; set; } = string.Empty;
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    
    public LineOfCredit? LineOfCredit { get; set; }

    /// <summary>
    /// Gets the full name of the customer.
    /// </summary>
    /// <returns>The full name in the format "FirstName LastName".</returns>
    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}