
namespace DKCrm.Shared.Models
{
    public class StampPosition
    { 
        public int PageNumber { get; set; }
        public double PercentOfBottomEdge { get; set; }
        public double PercentOfLeftEdge { get; set; }
        public string StampImage { get; set; } = null!;
    }
}
