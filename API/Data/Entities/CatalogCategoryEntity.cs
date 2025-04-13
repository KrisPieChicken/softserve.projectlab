using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CatalogCategoryEntity
{
    public int CatalogId { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public virtual CatalogEntity Catalog { get; set; } = null!;

    public virtual CategoryEntity Category { get; set; } = null!;
}
