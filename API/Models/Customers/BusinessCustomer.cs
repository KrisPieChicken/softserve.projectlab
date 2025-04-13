namespace API.Models.Customers;

/// <summary>
/// Represents a business customer with specialized business-related properties.
/// </summary>
public class BusinessCustomer : Customer
{
    public string CompanyName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tax identification number.
    /// </summary>
    public string TaxId { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the industry sector of the business.
    /// </summary>
    public string Industry { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of employees in the company.
    /// </summary>
    public int EmployeeCount { get; set; }

    /// <summary>
    /// Gets or sets the annual revenue of the business.
    /// </summary>
    public decimal AnnualRevenue { get; set; }

    /// <summary>
    /// Gets or sets the business size category (Small, Medium, Large, Enterprise).
    /// </summary>
    public string BusinessSize { get; set; } = "Small";

    /// <summary>
    /// Gets or sets the volume discount rate based on order size.
    /// </summary>
    public decimal VolumeDiscountRate { get; set; }

    /// <summary>
    /// Gets or sets the credit terms for the business (e.g., Net30, Net60).
    /// </summary>
    public string CreditTerms { get; set; } = "Net30";

    /// <summary>
    /// Calculates the volume discount for a given amount based on the total order value.
    /// </summary>
    /// <param name="orderValue">The total value of the order.</param>
    /// <returns>The discount amount.</returns>
    public decimal CalculateVolumeDiscount(decimal orderValue)
    {
        // Apply graduated volume discounts based on order size
        if (orderValue >= 10000)
        {
            return orderValue * (VolumeDiscountRate + 5) / 100;
        }
        else if (orderValue >= 5000)
        {
            return orderValue * (VolumeDiscountRate + 2) / 100;
        }
        else if (orderValue >= 1000)
        {
            return orderValue * VolumeDiscountRate / 100;
        }
        
        return 0;
    }
}