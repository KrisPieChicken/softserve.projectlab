using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softserve.projectlabs.Shared.DTOs.Supplier
{
    public class AddItemToSupplierDto
    {
        [Required]
        public int Sku { get; set; }
    }

}
