﻿using System;
using System.Collections.Generic;

namespace API.Data.Entities;

public partial class PackageItemEntity
{
    public int PackageId { get; set; }

    public int Sku { get; set; }

    public int ItemQuantity { get; set; }

    public string? Notes { get; set; }

    public int? WarrantyMonths { get; set; }

    public string? SerialNumber { get; set; }

    public virtual PackageEntity Package { get; set; } = null!;

    public virtual ItemEntity SkuNavigation { get; set; } = null!;
}
