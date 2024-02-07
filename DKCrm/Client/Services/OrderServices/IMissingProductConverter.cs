using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public interface IMissingProductConverter
    {
        public List<MissingProductInCatalog> ConvertMissingProductStringToList(string joinedString);
        public string? ConvertMissingProductListToString(List<MissingProductInCatalog>? list);
    }
}
