using API.Models.IntAdmin;

namespace API.Models.Customers;

/// <summary>
/// Represents a package of products or services sold to a customer.
/// </summary>
public class Package
{
    /// <summary>
    /// Gets or sets the unique identifier for the package.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the name of the package.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the description of the package.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date when the package was sold.
    /// </summary>
    public DateTime SaleDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the list of items included in the package.
    /// </summary>
    public List<PackageItem> Items { get; set; } = new List<PackageItem>();

    /// <summary>
    /// Gets or sets the customer who purchased the package.
    /// </summary>
    public Customer Customer { get; set; } = new Customer();

    /// <summary>
    /// Gets or sets the package status (e.g., Processing, Shipped, Delivered, Canceled).
    /// </summary>
    public string Status { get; set; } = "Processing";

    /// <summary>
    /// Gets or sets the contract ID associated with this package.
    /// </summary>
    public string? ContractId { get; set; }

    /// <summary>
    /// Gets or sets the contract term in months.
    /// </summary>
    public int ContractTermMonths { get; set; }

    /// <summary>
    /// Gets or sets the contract start date.
    /// </summary>
    public DateTime ContractStartDate { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Gets or sets the contract end date.
    /// </summary>
    public DateTime ContractEndDate => ContractStartDate.AddMonths(ContractTermMonths);

    /// <summary>
    /// Gets or sets the monthly subscription fee for this package.
    /// </summary>
    public decimal MonthlyFee { get; set; }

    /// <summary>
    /// Gets or sets the one-time setup fee for this package.
    /// </summary>
    public decimal SetupFee { get; set; }

    /// <summary>
    /// Gets or sets the discount applied to this package.
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// Gets or sets the payment method used for this package.
    /// </summary>
    public string PaymentMethod { get; set; } = "Credit Card";

    /// <summary>
    /// Gets or sets the shipping address for this package.
    /// </summary>
    public string ShippingAddress { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the tracking number for the shipment.
    /// </summary>
    public string? TrackingNumber { get; set; }

    /// <summary>
    /// Gets or sets the estimated delivery date.
    /// </summary>
    public DateTime? EstimatedDeliveryDate { get; set; }

    /// <summary>
    /// Gets or sets the actual delivery date.
    /// </summary>
    public DateTime? ActualDeliveryDate { get; set; }

    /// <summary>
    /// Gets or sets whether this package is a renewal of a previous package.
    /// </summary>
    public bool IsRenewal { get; set; }

    /// <summary>
    /// Gets or sets the ID of the previous package if this is a renewal.
    /// </summary>
    public string? PreviousPackageId { get; set; }

    /// <summary>
    /// Gets or sets the list of notes associated with this package.
    /// </summary>
    public List<PackageNote> Notes { get; set; } = new List<PackageNote>();

    /// <summary>
    /// Calculates the total price of the package before discount.
    /// </summary>
    /// <returns>The sum of all item prices multiplied by their quantities.</returns>
    public decimal CalculateTotalPrice()
    {
        return Items.Sum(item => item.Item.ItemUnitCost * item.Quantity);
    }

    /// <summary>
    /// Calculates the total price after applying the discount.
    /// </summary>
    /// <returns>The discounted total price.</returns>
    public decimal CalculateDiscountedPrice()
    {
        return CalculateTotalPrice() - DiscountAmount;
    }

    /// <summary>
    /// Calculates the total cost of the package over the contract term.
    /// </summary>
    /// <returns>The total cost including setup fee and monthly fees.</returns>
    public decimal CalculateTotalContractValue()
    {
        return SetupFee + (MonthlyFee * ContractTermMonths) - DiscountAmount;
    }

    /// <summary>
    /// Adds an item to the package.
    /// </summary>
    /// <param name="item">The item to add to the package.</param>
    /// <param name="quantity">The quantity to add. Must be positive.</param>
    /// <exception cref="ArgumentException">Thrown when the quantity is not positive.</exception>
    public void AddItem(Item item, int quantity)
    {
        if (quantity <= 0)
        {
            throw new ArgumentException("Quantity must be positive", nameof(quantity));
        }

        var existingItem = Items.FirstOrDefault(i => i.Item == item);
        
        if (existingItem != null)
        {
            existingItem.Quantity += quantity;
        }
        else
        {
            Items.Add(new PackageItem { Item = item, Quantity = quantity });
        }
    }

    /// <summary>
    /// Adds a note to the package.
    /// </summary>
    /// <param name="title">The title of the note.</param>
    /// <param name="content">The content of the note.</param>
    /// <param name="createdBy">The user who created the note.</param>
    public void AddNote(string title, string content, string createdBy)
    {
        Notes.Add(new PackageNote
        {
            Title = title,
            Content = content,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        });
    }

    /// <summary>
    /// Updates the status of the package.
    /// </summary>
    /// <param name="status">The new status of the package.</param>
    /// <param name="updatedBy">The user who updated the status.</param>
    /// <param name="notes">Optional notes about the status update.</param>
    public void UpdateStatus(string status, string updatedBy, string? notes = null)
    {
        Status = status;
        
        if (!string.IsNullOrEmpty(notes))
        {
            AddNote($"Status Update: {status}", notes, updatedBy);
        }
    }

    /// <summary>
    /// Checks if the contract is still active.
    /// </summary>
    /// <returns>True if the contract is active, false otherwise.</returns>
    public bool IsContractActive()
    {
        return DateTime.UtcNow <= ContractEndDate;
    }

    /// <summary>
    /// Calculates the remaining time on the contract.
    /// </summary>
    /// <returns>The remaining time as a TimeSpan.</returns>
    public TimeSpan GetRemainingContractTime()
    {
        return ContractEndDate > DateTime.UtcNow ? ContractEndDate - DateTime.UtcNow : TimeSpan.Zero;
    }
}

/// <summary>
/// Represents an item in a package with its quantity.
/// </summary>
public class PackageItem
{
    /// <summary>
    /// Gets or sets the item.
    /// </summary>
    public Item Item { get; set; } = null!;

    /// <summary>
    /// Gets or sets the quantity of the item.
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// Gets or sets any special notes for this item.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Gets or sets the warranty period in months for this item.
    /// </summary>
    public int? WarrantyMonths { get; set; }

    /// <summary>
    /// Gets or sets the serial number of the item if applicable.
    /// </summary>
    public string? SerialNumber { get; set; }
}

/// <summary>
/// Represents a note associated with a package.
/// </summary>
public class PackageNote
{
    /// <summary>
    /// Gets or sets the unique identifier for the note.
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Gets or sets the title of the note.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the note.
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the user who created the note.
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the note was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}