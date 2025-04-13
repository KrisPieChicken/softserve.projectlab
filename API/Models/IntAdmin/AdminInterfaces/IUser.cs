using API.Data.Entities;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin.Interfaces
{
    public interface IUser
    {
        int UserId { get; set; }
        string UserEmail { get; set; }
        string UserFirstName { get; set; }
        string UserLastName { get; set; }
        string UserPhone { get; set; }
        string UserPassword { get; set; }
        bool UserStatus { get; set; }
        string UserImage { get; set; }
        int BranchId { get; set; }
    }
}
