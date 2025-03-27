using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Customers;

/// <summary>
/// Represents a line of credit with a balance that can be manipulated through deposits and withdrawals.
/// </summary>
public class LineOfCredit
{
    private decimal _balance;
    
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    // FK to Customer
    public string CustomerId { get; set; }

    [StringLength(100)]
    public string Provider { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal CreditLimit { get; set; }

    [Column(TypeName = "decimal(10,5)")]
    public decimal AnnualInterestRate { get; set; }

    public DateTime OpenDate { get; set; } = DateTime.UtcNow;
    public DateTime NextPaymentDueDate { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal MinimumPaymentAmount { get; set; }

    public int CreditScore { get; set; }

    [StringLength(20)]
    public string Status { get; set; } = "Active";

    public DateTime LastReviewDate { get; set; } = DateTime.UtcNow;

    // Navigation property to related transactions
    public ICollection<CreditTransaction> Transactions { get; set; } = new List<CreditTransaction>();

    /// <summary>
    /// Gets the current balance of the line of credit.
    /// </summary>
    /// <returns>The current balance.</returns>
    public decimal GetBalance()
    {
        return _balance;
    }

    // Resto de métodos...
}