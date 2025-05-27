namespace softserve.projectlabs.Shared.DTOs.Branch
{
    public class BranchDto : BaseDto
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; } = string.Empty;
        public string BranchCity { get; set; } = string.Empty;
        public string BranchAddress { get; set; } = string.Empty;
        public string BranchRegion { get; set; } = string.Empty;
        public string BranchContactNumber { get; set; } = string.Empty;
        public string BranchContactEmail { get; set; } = string.Empty;
    }
}