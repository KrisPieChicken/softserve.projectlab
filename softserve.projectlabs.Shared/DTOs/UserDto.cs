namespace softserve.projectlabs.Shared.DTOs
{
    public class UserDto : BaseDto
    {
        public int? UserId { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string UserFirstName { get; set; } = string.Empty;
        public string UserLastName { get; set; } = string.Empty;
        public string UserPhone { get; set; } = string.Empty;
        public string UserPassword { get; set; } = string.Empty;
        public bool UserStatus { get; set; }
        public string? UserImage { get; set; }
        public int BranchId { get; set; }

        // List of role IDs to be assigned to the user.
        public List<int> RoleIds { get; set; } = new List<int>();
    }
}
