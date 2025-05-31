using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softserve.projectlabs.Shared.DTOs.Package;

public class PackageDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime SaleDate { get; set; }
    public List<PackageItemDto> Items { get; set; } = new List<PackageItemDto>();
    public List<PackageNoteDto> Notes { get; set; } = new List<PackageNoteDto>();
    public string? ContractId { get; set; }
    public int ContractTermMonths { get; set; }
    public DateTime ContractStartDate { get; set; }
    public DateTime ContractEndDate { get; set; }
    public decimal MonthlyFee { get; set; }
    public decimal SetupFee { get; set; }
    public decimal DiscountAmount { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string ShippingAddress { get; set; } = string.Empty;
    public string? TrackingNumber { get; set; }
    public DateTime? EstimatedDeliveryDate { get; set; }
    public DateTime? ActualDeliveryDate { get; set; }
    public bool IsRenewal { get; set; }
    public string? PreviousPackageId { get; set; }
}
