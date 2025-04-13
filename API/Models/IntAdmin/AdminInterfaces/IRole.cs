using API.Data.Entities;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin.Interfaces
{

    public interface IRole
    {
        int RoleId { get; set; }
        string RoleName { get; set; }
        string RoleDescription { get; set; }
        bool RoleStatus { get; set; }
    }
}
