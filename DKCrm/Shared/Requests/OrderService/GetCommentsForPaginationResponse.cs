namespace DKCrm.Shared.Requests.OrderService;

public class GetCommentsForPaginationResponse <T>
{
    public IEnumerable<T>? Items { get; set; }
    public int TotalItems { get; set; }
}