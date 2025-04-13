using API.Models.IntAdmin;
using System;
using System.Collections.Generic;

namespace API.Models.Logistics.Interfaces
{
    public interface ISupplierOrder
    {
        int OrderId { get; set; }
        int SupplierId { get; set; }
        List<Item> OrderedItems { get; set; }
        DateTime OrderDate { get; set; }
        DateTime? ExpectedDeliveryDate { get; set; }
        string Status { get; set; }
    }
}
