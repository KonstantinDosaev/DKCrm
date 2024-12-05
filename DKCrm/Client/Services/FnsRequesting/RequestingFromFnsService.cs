using DKCrm.Shared.Models.CompanyModels;
using System.Text.Json;
using DKCrm.Client.Services.InternalCompanyDataService;
using static DKCrm.Client.Services.FnsRequesting.FnsModels;

namespace DKCrm.Client.Services.FnsRequesting
{
    public class RequestingFromFnsService : IRequestingFromFnsService
    {
        private readonly HttpClient _httpClient;

        public RequestingFromFnsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<FnsRequest>> GetCompanyByInn(string inn)
        {
            var internalData = await new InternalCompanyDataManager(_httpClient).GetAsync();
            IEnumerable<FnsRequest> result;
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://api-fns.ru/api/egr?req={inn}&key={internalData.KeyFns}");
            //request.Headers.Add("Accept", "/");
            //request.Headers.Add("User-Agent", "HttpClientFactory-Sample");

            //var client = ClientFactory.CreateClient();

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                var branches = await JsonSerializer.DeserializeAsync
                    <Rootobject>(responseStream);
                result = FnsConverter(branches!);
            }
            else
            {
                result = new List<FnsRequest>(){new FnsRequest(){Name = "Ошибка запроса"}};
            }
           
            

            return result;


        }
        internal IEnumerable<FnsRequest> FnsConverter(Rootobject tempFnsRequest)
        {
            var result = new List<FnsRequest>();
            foreach (var item in tempFnsRequest.items)
            {
                var company = new FnsRequest()
                {
                    Name = item.ЮЛ.НаимПолнЮЛ,
                    INN = item.ЮЛ.ИНН,
                    ORGN = item.ЮЛ.ОГРН,
                    KPP = item.ЮЛ.КПП,
                    PostalCode = item.ЮЛ.Адрес.Индекс,
                    Phone = item.ЮЛ.Контакты.Телефон[0],
                    Director = item.ЮЛ.Руководитель.ФИОПолн,
                    Revenue = item.ЮЛ.Капитал.СумКап,
                    LegalAddress = item.ЮЛ.Адрес.АдресПолн,
                    OKVED = $"{item.ЮЛ.ОснВидДеят.Код} {item.ЮЛ.ОснВидДеят.Текст}"
                };
                result.Add(company);
            }
            

            return result;
        }
    }

   

}
