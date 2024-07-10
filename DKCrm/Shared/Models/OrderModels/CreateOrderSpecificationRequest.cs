namespace DKCrm.Shared.Models.OrderModels;

public class CreateOrderSpecificationRequest
{
    public Guid OrderId { get; set; }
    public Guid BuyerCertifyingPersonId { get; set; }
    public Guid SellerCertifyingPersonId { get; set;  }
    public string NamingCondition = null!;
    public ICollection<string>? Conditions { get; set; }
}