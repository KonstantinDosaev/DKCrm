namespace DKCrm.Shared.Requests
{
    public class MergeImportedProductsRequest
    {
        public Guid PrimaryImportedProductId { get; set; } 

        public Guid SecondaryImportedProductId { get; set; }
    }
}
