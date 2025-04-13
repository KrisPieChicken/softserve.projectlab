using API.Data.Entities;
using System.Collections.Generic;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin.Interfaces
{
    /// <summary>
    /// Represents a category with methods for CRUD operations.
    /// </summary>
    public interface ICategory
    {
        int CategoryId { get; set; }
        string CategoryName { get; set; }
        bool CategoryStatus { get; set; }
    }
}
