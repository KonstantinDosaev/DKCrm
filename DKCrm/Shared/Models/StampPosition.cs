
namespace DKCrm.Shared.Models
{
    public class StampPosition
    { 
        public Guid Id { get; set; }
        public int PageNumber { get; set; }
        public double PercentOfBottomEdge { get; set; }
        public double PercentOfLeftEdge { get; set; }
        public string? StampImageName { get; set; }
        public byte[]? StampImage { get; set; }
    }
}
