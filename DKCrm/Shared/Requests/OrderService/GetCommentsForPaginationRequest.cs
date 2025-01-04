namespace DKCrm.Shared.Requests.OrderService;

public class GetCommentsForPaginationRequest
{
    public Guid ComponentOwnerId { get; set; }
    public bool GetOnlyWarningComments { get; set; }
    public int PageIndex { get; set;}
    public int PageSize { get; set;}
}