using API.Models.IntAdmin.AdminInterfaces;

namespace API.Models.Customers;

/// <summary>
/// Represents a customer with personal and financial information.
/// </summary>
public class Customer : BaseEntity
{
    public int CustomerId { get; set; }
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public string CustomerContactNumber { get; set; } = null!;
    public string CustomerContactEmail { get; set; } = null!;
    public DateOnly? BirthDate { get; set; }
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;

    /// <summary>
    /// Gets the full name of the customer.
    /// </summary>
    /// <returns>The full name in the format "FirstName LastName".</returns>
    public string GetFullName()
    {
        return $"{CustomerFirstName} {CustomerLastName}".Trim();
    }
}