using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Customers;

/// <summary>
/// Represents a transaction in a line of credit.
/// </summary>
public class CreditTransaction
{
    [Key]
    public string Id { get; set; } = Guid.NewGuid().ToString();

    [Required]
    [StringLength(50)]
    public string TransactionType { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [StringLength(255)]
    public string Description { get; set; } = string.Empty;

    public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

    // Relación con LineOfCredit - Solo una clave foránea
    public string LineOfCreditId { get; set; }
    
    // Propiedad de navegación
    [ForeignKey("LineOfCreditId")]
    public LineOfCredit LineOfCredit { get; set; }
}