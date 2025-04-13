namespace API.Models.Customers;

/// <summary>
/// Represents an individual customer with basic consumer-related properties.
/// </summary>
public class IndividualCustomer : Customer
{
    /// <summary>
    /// Gets or sets whether the customer is eligible for promotions.
    /// </summary>
    public bool IsEligibleForPromotions { get; set; } = true;

    /// <summary>
    /// Gets or sets the customer's preference for communication (Email, SMS, Call).
    /// </summary>
    public string CommunicationPreference { get; set; } = "Email";

    /// <summary>
    /// Gets or sets the loyalty points accumulated by the customer.
    /// </summary>
    public int LoyaltyPoints { get; set; }

    /// <summary>
    /// Gets or sets the date of the last purchase.
    /// </summary>
    public DateTime? LastPurchaseDate { get; set; }

    /// <summary>
    /// Adds loyalty points to the customer's account based on purchase amount.
    /// </summary>
    /// <param name="purchaseAmount">The amount of the purchase.</param>
    /// <returns>The number of points added.</returns>
    public int AddLoyaltyPoints(decimal purchaseAmount)
    {
        // For example, 1 point per $10 spent
        int pointsToAdd = (int)(purchaseAmount / 10);
        LoyaltyPoints += pointsToAdd;
        return pointsToAdd;
    }

    /// <summary>
    /// Redeems loyalty points for a discount.
    /// </summary>
    /// <param name="pointsToRedeem">The number of points to redeem.</param>
    /// <returns>The discount amount ($1 for every 100 points).</returns>
    /// <exception cref="InvalidOperationException">Thrown when there are insufficient points.</exception>
    public decimal RedeemLoyaltyPoints(int pointsToRedeem)
    {
        if (pointsToRedeem > LoyaltyPoints)
        {
            throw new InvalidOperationException("Insufficient loyalty points");
        }

        LoyaltyPoints -= pointsToRedeem;
        
        // For example, $1 discount for every 100 points
        return pointsToRedeem / 100.0m;
    }
}