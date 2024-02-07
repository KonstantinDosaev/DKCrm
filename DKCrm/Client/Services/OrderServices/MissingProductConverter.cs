using System.Text;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.OrderServices
{
    public class MissingProductConverter : IMissingProductConverter
    {
        public List<MissingProductInCatalog> ConvertMissingProductStringToList(string joinedString)
        {
            var result = new List<MissingProductInCatalog>();
            var splitArr = joinedString.Split('>');
            foreach (var missSting in splitArr)
            {
                var splitToItems = missSting.Split('^');

                result.Add(new MissingProductInCatalog
                {
                    PartNumber = splitToItems[0],
                    Name = splitToItems[1],
                    BrandName = splitToItems[2],
                    Quantity = Convert.ToInt32(splitToItems[3])
                });
            }
            return result;
        }
        public string? ConvertMissingProductListToString(List<MissingProductInCatalog>? list)
        {
            if (list == null)
            {
                return null;
            }
            var result = new StringBuilder();
            foreach (var item in list)
            {
                if (result.Length > 0)
                {
                    result.Append(">");
                }
                result.Append($"{item.PartNumber}^{item.Name}^{item.BrandName}^{item.Quantity}");
            }
            return result.ToString();
        }
    }
}
