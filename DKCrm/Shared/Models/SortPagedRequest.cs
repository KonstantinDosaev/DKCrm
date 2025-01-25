using DKCrm.Shared.Constants;
using MudBlazor;

namespace DKCrm.Shared.Models
{
    public class SortPagedRequest<T>
    {
        public string? SearchString { get; set; }
        public string? SortLabel { get; set; }
        public SortDirection? SortDirection { get; set; }
        public string? SearchInChapter { get; set; }
        public int PageIndex { get; set;}
        public int PageSize { get; set;}
        public string? Chapter { get; set; }
        public Guid? ChapterId { get; set; }
        public T? FilterTuple{ get; set; }
        public string? GlobalFilterValue { get; set;}
        public GlobalFilterTypes GlobalFilterType { get; set;}
    }
}
