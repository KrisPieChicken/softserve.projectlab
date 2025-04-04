using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class CatalogEntity
{
    public int CatalogId { get; set; }
    public required string CatalogName { get; set; }
    public required string CatalogDescription { get; set; }
    public bool CatalogStatus { get; set; }

    public virtual ICollection<CatalogCategoryEntity> CatalogCategoryEntities { get; set; } = new List<CatalogCategoryEntity>();

}
