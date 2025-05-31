using API.Models.IntAdmin.AdminInterfaces;
using softserve.projectlabs.Shared.Utilities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace API.Models.IntAdmin;

public class Catalog : BaseEntity, ICatalog
{
    public int CatalogID { get; set; }
    public string CatalogName { get; set; } = string.Empty;
    public string CatalogDescription { get; set; } = string.Empty;
    public bool CatalogStatus { get; set; }

    public List<ICategory> Categories { get; set; } = new List<ICategory>();

}
