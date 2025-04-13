using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CategoryEntity
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public bool CategoryStatus { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<CatalogCategoryEntity> CatalogCategoryEntities { get; set; } = new List<CatalogCategoryEntity>();

    public virtual ICollection<ItemEntity> ItemEntities { get; set; } = new List<ItemEntity>();
}
