namespace API.Models.Customers;

/// <summary>
/// Represents a premium customer with additional benefits.
/// </summary>
public class PremiumCustomer : Customer
{
    /// <summary>
    /// Gets or sets the discount rate applied to premium customers.
    /// </summary>
    public decimal DiscountRate { get; set; }

    /// <summary>
    /// Gets or sets the date when the premium membership started.
    /// </summary>
    public DateTime MembershipStartDate { get; set; }

    /// <summary>
    /// Gets or sets the date when the premium membership expires.
    /// </summary>
    public DateTime MembershipExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets the premium tier level (e.g., Silver, Gold, Platinum).
    /// </summary>
    public string TierLevel { get; set; } = "Silver";

    /// <summary>
    /// Calculates the discounted price for a given amount based on the premium discount rate.
    /// </summary>
    /// <param name="originalPrice">The original price before discount.</param>
    /// <returns>The discounted price.</returns>
    public decimal CalculateDiscountedPrice(decimal originalPrice)
    {
        return originalPrice * (1 - DiscountRate / 100);
    }

    /// <summary>
    /// Checks if the premium membership is active.
    /// </summary>
    /// <returns>True if the membership is active, false otherwise.</returns>
    public bool IsMembershipActive()
    {
        return DateTime.UtcNow <= MembershipExpiryDate;
    }
}