using DKCrm.Shared.Models.Products;
using Microsoft.OData.UriParser;

namespace DKCrm.Shared.Models
{
    public class SortPagedRequest
    {
        public string? SearchString { get; set; }
        public string? SortLabel { get; set; }
        public OrderByDirection? SortDirection { get; set; }
        public int PageIndex { get; set;}
        public int PageSize { get; set;}
        public string? Chapter { get; set; }
        public Guid? ChapterId { get; set; }
        public FilterProductTuple? FilterTuple{ get; set; }
    }
}
