using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace softserve.projectlabs.Shared.DTOs.Customer;
public class CustomerCreateDto
{
    public string CustomerFirstName { get; set; } = null!;
    public string CustomerLastName { get; set; } = null!;
    public string CustomerContactNumber { get; set; } = null!;
    public string CustomerContactEmail { get; set; } = null!;
    public DateOnly? BirthDate { get; set; }
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string ZipCode { get; set; } = null!;
}
