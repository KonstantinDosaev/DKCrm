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

        public async Task<List<(string charCod, decimal?)>> CurrencyConverter(string currencyToConvert,
            decimal? firstPrice,
            double? currencyPercent,
            IEnumerable<string> currencyCharCodesInConvert)
        {
        
            var result = new List<(string charCod, decimal? value)>() { };
            var elements = await GetCurrencyXml();

            var cbCharValue = elements.Select(s =>new{
            
                charCode=s.Element("CharCode")!.Value,
                value =Convert.ToDecimal(s.Element("Value")!.Value)
            }).Where(w=>currencyCharCodesInConvert.Contains(w.charCode)||w.charCode==currencyToConvert).ToList();
            foreach (var charCodIn in currencyCharCodesInConvert)
            {
                if (currencyToConvert=="RUB")
                {
                    if (charCodIn=="RUB")
                    {
                        result.Add((charCodIn, firstPrice));
                    }
                    else
                    {
                        var t = cbCharValue.FirstOrDefault(f => f.charCode == charCodIn).value;
                        var course = (decimal)1 /(decimal)t;
                        result.Add((charCodIn, firstPrice * (course + (course / 100 * (decimal)currencyPercent!))));
                    }
                   
                }
                else
                {
                    decimal course;
                    if (charCodIn =="RUB")
                    {
                        course =  cbCharValue.FirstOrDefault(f => f.charCode == currencyToConvert)!.value;
                    }
                    else
                    {
                        course = cbCharValue.FirstOrDefault(f => f.charCode == currencyToConvert)!.value /
                            cbCharValue.FirstOrDefault(f => f.charCode == charCodIn)!.value;
                    }
                    result.Add((charCodIn, firstPrice*(course + (course / 100 * (decimal)currencyPercent!))));
                }
            }

            return result;
        }
    }

}
