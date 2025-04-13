namespace softserve.projectlabs.Shared.DTOs
{
    public class BaseDto
    {
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
