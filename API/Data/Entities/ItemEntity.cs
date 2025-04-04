using System.Collections.Generic;

namespace API.Data.Entities;

public partial class ItemEntity
{
    public int Sku { get; set; }

    public required string ItemName { get; set; }
    public required string ItemDescription { get; set; }
    public int OriginalStock { get; set; }
    public int CurrentStock { get; set; }
    public required string ItemCurrency { get; set; }
    public decimal ItemUnitCost { get; set; }
    public decimal ItemMarginGain { get; set; }
    public decimal? ItemDiscount { get; set; }
    public decimal? ItemAdditionalTax { get; set; }
    public decimal ItemPrice { get; set; }
    public bool ItemStatus { get; set; }
    public int CategoryId { get; set; }
    public string? ItemImage { get; set; }

    public virtual CategoryEntity Category { get; set; } = null!;
    public virtual ICollection<CartItemEntity> CartItemEntities { get; set; } = new List<CartItemEntity>();
    public virtual ICollection<OrderItemEntity> OrderItemEntities { get; set; } = new List<OrderItemEntity>();
    public virtual ICollection<PackageItemEntity> PackageItemEntities { get; set; } = new List<PackageItemEntity>();
    public virtual ICollection<SupplierItemEntity> SupplierItemEntities { get; set; } = new List<SupplierItemEntity>();
    public virtual ICollection<WarehouseItemEntity> WarehouseItemEntities { get; set; } = new List<WarehouseItemEntity>();
}
