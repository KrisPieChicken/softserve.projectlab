using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class PackageEntity
{
    public int PackageId { get; set; }

    public string PackageName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual ICollection<PackageItemEntity> PackageItemEntities { get; set; } = new List<PackageItemEntity>();

    public virtual ICollection<PackageNoteEntity> PackageNoteEntities { get; set; } = new List<PackageNoteEntity>();
}
