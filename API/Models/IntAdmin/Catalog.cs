using System.Collections.Generic;
using System.Linq;
using API.Models.IntAdmin.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin
{
    public class Catalog : ICatalog
    {
        public int CatalogID { get; set; }
        public string CatalogName { get; set; } = string.Empty;
        public string CatalogDescription { get; set; } = string.Empty;
        public bool CatalogStatus { get; set; }

        public List<ICategory> Categories { get; set; } = new List<ICategory>();
    }
}
