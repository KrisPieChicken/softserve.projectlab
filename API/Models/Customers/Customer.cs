using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Customers;

/// <summary>
/// Represents a customer with personal and financial information.
/// </summary>
public class Customer
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;
    
    public DateOnly BirthDate { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;
    
    [StringLength(100)]
    public string City { get; set; } = string.Empty;
    
    [StringLength(50)]
    public string State { get; set; } = string.Empty;
    
    [StringLength(20)]
    public string ZipCode { get; set; } = string.Empty;
    
    public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
    
    public LineOfCredit? LineOfCredit { get; set; }

    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";

    /// <summary>
    /// Gets the full name of the customer.
    /// </summary>
    /// <returns>The full name in the format "FirstName LastName".</returns>
    public string GetFullName()
    {
        return $"{FirstName} {LastName}";
    }
}