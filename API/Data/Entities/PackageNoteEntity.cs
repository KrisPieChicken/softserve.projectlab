using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class PackageNoteEntity
{
    public string Id { get; set; } = null!;

    public int PackageId { get; set; }

    public string Title { get; set; } = null!;

    public string Content { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public virtual PackageEntity Package { get; set; } = null!;
}
