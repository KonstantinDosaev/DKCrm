namespace DKCrm.Shared.Requests.OrderService
{
    public class GetWarningCommentsToOrderRequest
    {
        public bool GetOnlyNotOpen { get; set; }
        public string? OrderType { get; set; }
        public int PageIndex { get; set;}
        public int PageSize { get; set;}
    }
}
