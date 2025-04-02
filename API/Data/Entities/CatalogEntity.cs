﻿using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CatalogEntity
{
    public int CatalogId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? Status { get; set; }

    public virtual ICollection<CatalogCategoryEntity> CatalogCategories { get; set; } = new List<CatalogCategoryEntity>();
}


