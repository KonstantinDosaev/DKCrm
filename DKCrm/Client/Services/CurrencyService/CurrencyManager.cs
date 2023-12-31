using System.Xml.Linq;
using System.Globalization;
using System.Text;

namespace DKCrm.Client.Services.CurrencyService
{
    public class CurrencyManager: ICurrencyManager
    {
        private readonly HttpClient _httpClient;

        public CurrencyManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<(string charCode,string name)>> GetCurrencyCharCode()
        {
            var result = new List<(string charCode, string name)>(){("RUB","Российский рубль")};
            var xml = await _httpClient.GetByteArrayAsync("https://www.cbr-xml-daily.ru/daily.xml");
            var utfString = CodePagesEncodingProvider.Instance.GetEncoding(1251)!.GetString(xml);
            var xdoc = XDocument.Parse(utfString);
            var el = xdoc.Element("ValCurs").Elements("Valute");
            foreach (var item in el)
            {
                result.Add((item.Element("CharCode")!.Value, item.Element("Name")!.Value.ToString(CultureInfo.InstalledUICulture)));
            }
           
            return result.OrderBy(o => o.charCode).ToList();
        }
    }

}
