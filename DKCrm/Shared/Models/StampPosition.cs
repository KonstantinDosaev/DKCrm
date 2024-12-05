
namespace DKCrm.Shared.Models
{
    public class StampPosition
    { 
        public Guid Id { get; set; }
        public int PageNumber { get; set; }
        public double PercentOfBottomEdge { get; set; }
        public double PercentOfLeftEdge { get; set; }
        public Guid? StampImageId { get; set; }
        public string? StampImagePath { get; set; }
        public byte[]? StampImage { get; set; }
    }
}
