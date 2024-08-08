using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Shared.Models.OrderModels;

public class CreateOrderSpecificationRequest
{
    public Guid OrderId { get; set; }
    public Employee BuyerCertifyingPersonId { get; set; } = null!;
    public Employee SellerCertifyingPersonId { get; set;  } = null!;
    public int DaysOfDelivery { get; set; }
    public ICollection<StringToFonts> NamingCondition { get; set; } = null!;
    public ICollection<StringToFonts> Conditions { get; set; } = null!;
}