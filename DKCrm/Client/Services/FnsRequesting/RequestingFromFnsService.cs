using DKCrm.Shared.Models.CompanyModels;
using System.Net.Http.Json;
using System.Text.Json;
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
            IEnumerable<FnsRequest> result;
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://api-fns.ru/api/egr?req={inn}&key=818f803787bccb7aaefd1c6098d10f0713571d92");
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
