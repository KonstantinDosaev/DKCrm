namespace DKCrm.Shared.Models.CompanyModels;

public class FilterCompanyTuple
{
    public Guid? CompanyTypeId { get; set; }
    public bool IsItemsWithUnreadComments { get; set; }

}