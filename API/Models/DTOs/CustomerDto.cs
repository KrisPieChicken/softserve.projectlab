using System;

namespace API.Models.DTOs
{
    public class CustomerDto
    {
        // Campo discriminador para saber qué tipo de cliente crear
        public string CustomerType { get; set; } = "Individual"; // "Business", "Individual", "Premium"
        
        // Propiedades básicas de Customer
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        
        // Propiedades de BusinessCustomer
        public string CompanyName { get; set; } = string.Empty;
        public string TaxId { get; set; } = string.Empty;
        public string Industry { get; set; } = string.Empty;
        public int EmployeeCount { get; set; }
        public decimal AnnualRevenue { get; set; }
        public string BusinessSize { get; set; } = "Small";
        public decimal VolumeDiscountRate { get; set; }
        public string CreditTerms { get; set; } = "Net30";
        
        // Propiedades de IndividualCustomer
        public bool IsEligibleForPromotions { get; set; } = true;
        public string CommunicationPreference { get; set; } = "Email";
        public int LoyaltyPoints { get; set; }
        public DateTime? LastPurchaseDate { get; set; }
        
        // Propiedades de PremiumCustomer
        public decimal DiscountRate { get; set; }
        public DateTime MembershipStartDate { get; set; } = DateTime.UtcNow;
        public DateTime MembershipExpiryDate { get; set; } = DateTime.UtcNow.AddYears(1);
        public string TierLevel { get; set; } = "Silver";
    }
}