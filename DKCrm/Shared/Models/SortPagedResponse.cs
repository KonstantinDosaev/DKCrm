

namespace DKCrm.Shared.Models
{
    public class SortPagedResponse <T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalItems { get; set; }
    }
}
