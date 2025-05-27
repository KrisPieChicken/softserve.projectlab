using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace softserve.projectlabs.Shared.DTOs.Warehouse
{
    public class WarehouseDto :BaseDto
    {
        [JsonIgnore]
        public int WarehouseId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public int Capacity { get; set; }

        public int BranchId { get; set; }
    }
}
