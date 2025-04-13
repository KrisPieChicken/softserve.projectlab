
namespace API.Models.IntAdmin.Interfaces
{
    /// <summary>
    /// Interface representing an Item.
    /// </summary>
    public interface IItem
    {
        int ItemId { get; set; }
        int Sku { get; set; }
        string ItemName { get; set; }
        string ItemDescription { get; set; }
        int OriginalStock { get; set; }
        int CurrentStock { get; set; }
        string ItemCurrency { get; set; }
        decimal ItemUnitCost { get; set; }
        decimal ItemMarginGain { get; set; }
        decimal? ItemDiscount { get; set; }
        decimal? ItemAdditionalTax { get; set; }
        decimal ItemPrice { get; set; }
        bool ItemStatus { get; set; }
        int CategoryId { get; set; }
        string? ItemImage { get; set; }
    }
}
