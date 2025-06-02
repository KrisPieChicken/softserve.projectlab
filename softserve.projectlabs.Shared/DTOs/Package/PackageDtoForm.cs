namespace softserve.projectlabs.Shared.DTOs.Package
{
    public class PackageDtoForm
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string SaleDateString { get; set; } = string.Empty;
        public string ContractId { get; set; } = string.Empty;
        public string ContractTermMonthsString { get; set; } = "12";
        public string ContractStartDateString { get; set; } = string.Empty;
        public string ContractEndDateString { get; set; } = string.Empty;
        public string MonthlyFeeString { get; set; } = "0";
        public string SetupFeeString { get; set; } = "0";
        public string DiscountAmountString { get; set; } = "0";
        public string PaymentMethod { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public string TrackingNumber { get; set; } = string.Empty;
        public string EstimatedDeliveryDateString { get; set; } = string.Empty;
        public string ActualDeliveryDateString { get; set; } = string.Empty;
        public bool IsRenewal { get; set; }
        public string PreviousPackageId { get; set; } = string.Empty;
        public List<PackageItemDto> Items { get; set; } = new();
        public string CustomerFirstName { get; set; } = string.Empty;
        public string CustomerLastName { get; set; } = string.Empty;
        public string CustomerContactEmail { get; set; } = string.Empty;
        public string CustomerContactNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;



        // Date fields for MudDatePicker binding
        public DateTime? SaleDate { get; set; }
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public DateTime? EstimatedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
    }
}
