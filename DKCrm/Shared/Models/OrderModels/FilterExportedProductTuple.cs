namespace DKCrm.Shared.Models.OrderModels
{
    public class FilterExportedProductTuple
    {
        public bool GroupByOrder { get; set; } 
        public bool IsNotComplete { get; set; }
        public FilterOrderTuple? FilterOrderTuple { get; set; }
    }
}
