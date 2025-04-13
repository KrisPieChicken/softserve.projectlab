using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class ItemEntity
{
    public int ItemId { get; set; }

    public int Sku { get; set; }

    public string ItemName { get; set; } = null!;

    public string ItemDescription { get; set; } = null!;

    public int OriginalStock { get; set; }

    public int CurrentStock { get; set; }

    public string ItemCurrency { get; set; } = null!;

    public decimal ItemUnitCost { get; set; }

    public decimal ItemMarginGain { get; set; }

    public decimal? ItemDiscount { get; set; }

    public decimal? ItemAdditionalTax { get; set; }

    public decimal ItemPrice { get; set; }

    public bool ItemStatus { get; set; }

    public int CategoryId { get; set; }

    public string? ItemImage { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CartItemEntity> CartItemEntities { get; set; } = new List<CartItemEntity>();

    public virtual CategoryEntity Category { get; set; } = null!;

    public virtual ICollection<OrderItemEntity> OrderItemEntities { get; set; } = new List<OrderItemEntity>();

    public virtual ICollection<PackageItemEntity> PackageItemEntities { get; set; } = new List<PackageItemEntity>();

    public virtual ICollection<SupplierItemEntity> SupplierItemEntities { get; set; } = new List<SupplierItemEntity>();

    public virtual ICollection<WarehouseItemEntity> WarehouseItemEntities { get; set; } = new List<WarehouseItemEntity>();
}
