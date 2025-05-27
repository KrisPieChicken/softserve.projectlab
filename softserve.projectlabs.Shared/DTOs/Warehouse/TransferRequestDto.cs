using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softserve.projectlabs.Shared.DTOs.Warehouse
{
    public class TransferRequestDto
    {
        public int SourceWarehouseId { get; set; }

        public int TargetWarehouseId { get; set; }

        public int Sku { get; set; }

        public int Quantity { get; set; }
    }

}
