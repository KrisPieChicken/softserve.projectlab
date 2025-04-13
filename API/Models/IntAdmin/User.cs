using API.Data.Entities;
using API.Models.IntAdmin.Interfaces;
using softserve.projectlabs.Shared.Utilities;

namespace API.Models.IntAdmin
{
    public class User : IUser
    {
        public int UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public bool UserStatus { get; set; }
        public string UserImage { get; set; } = string.Empty;
        public int BranchId { get; set; }

        // To adding roles to the user
        public List<IRole> Roles { get; set; } = new List<IRole>();
    }
}
