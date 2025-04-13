using System.Collections.Generic;
using API.Models.IntAdmin.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin
{
    public class Category : ICategory
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool CategoryStatus { get; set; }

        // The items associated with this category are represented as a list of Item objects.
        public List<Item> Items { get; set; } = new List<Item>();
    }
}
