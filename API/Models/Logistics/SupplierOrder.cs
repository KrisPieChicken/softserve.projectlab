using System;
using System.Collections.Generic;
using API.Models.IntAdmin;
using API.Models.Logistics.Interfaces;

namespace API.Models.Logistics
{
    public class SupplierOrder : ISupplierOrder
    {
        public int OrderId { get; set; }
        public int SupplierId { get; set; }
        public List<Item> OrderedItems { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime? ExpectedDeliveryDate { get; set; }
        public string Status { get; set; } = "Pending";

        public SupplierOrder() { }

        public SupplierOrder(int supplierId, List<Item> orderedItems)
        {
            OrderId = new Random().Next(1000, 9999);
            SupplierId = supplierId;
            OrderedItems = orderedItems;
            OrderDate = DateTime.UtcNow;
        }

        public void UpdateStatus(string newStatus)
        {
            Status = newStatus;
        }
    }
}
