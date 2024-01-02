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

        public async Task<IEnumerable<XElement>> GetCurrencyXml()
        {
            var xml = await _httpClient.GetByteArrayAsync("https://www.cbr-xml-daily.ru/daily.xml");
            var utfString = CodePagesEncodingProvider.Instance.GetEncoding(1251)!.GetString(xml);
            var xdoc = XDocument.Parse(utfString);
            var el = xdoc.Element("ValCurs")!.Elements("Valute");
            
            return el;
        }
        public async Task<List<(string charCode,string name)>> GetCurrencyCharCode()
        {
            var result = new List<(string charCode, string name)>(){("RUB","Российский рубль")};
            var elements = await GetCurrencyXml();
            foreach (var item in elements)
            {
                result.Add((item.Element("CharCode")!.Value, item.Element("Name")!.Value.ToString(CultureInfo.InstalledUICulture)));
            }
           
            return result.OrderBy(o => o.charCode).ToList();
        }

        public async Task<List<(string charCod, decimal?)>> CurrencyConverter(string firstCurrency, decimal? firstPrice,
            double currencyPercent,
            IEnumerable<string> currencyCharCodesToConvert)
        {
            var result = new List<(string charCod, decimal? value)>() { };
            var elements = await GetCurrencyXml();

            var cbCharValue = elements.Select(s =>new{
            
                charCode=s.Element("CharCode")!.Value,
                value =Convert.ToDecimal(s.Element("Value")!.Value)
            }).Where(w=>currencyCharCodesToConvert.Contains(w.charCode)||w.charCode==firstCurrency).ToList();
            foreach (var charCod in currencyCharCodesToConvert)
            {
                if (firstCurrency=="RUB")
                {
                    var cource = 1/cbCharValue.FirstOrDefault(f=>f.charCode==charCod)!.value;
                    result.Add((charCod, firstPrice*(cource+(decimal)currencyPercent)));
                }
                else
                {
                    decimal cource;
                    if (charCod=="RUB")
                    {
                         cource =  cbCharValue.FirstOrDefault(f => f.charCode == firstCurrency)!.value;
                    }
                    else
                    {
                        cource = cbCharValue.FirstOrDefault(f => f.charCode == firstCurrency)!.value /
                            cbCharValue.FirstOrDefault(f => f.charCode == charCod)!.value;
                    }
                    result.Add((charCod, firstPrice*(cource + (decimal)currencyPercent)));
                }
            }

            return result;
        }
    }

}
