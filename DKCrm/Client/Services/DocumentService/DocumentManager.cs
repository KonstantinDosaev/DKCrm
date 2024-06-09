using DKCrm.Shared.Models.Chat;
using System.Net.Http.Json;

namespace DKCrm.Client.Services.DocumentService
{
    public class DocumentManager: IDocumentManager
    {
        private readonly HttpClient _httpClient;

        public DocumentManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
       
        public async Task<string> CreateDoc()
        {
            var ytr = await _httpClient.GetFromJsonAsync<string>($"api/Document/GetDoc") ?? throw new InvalidOperationException();
            return ytr;

        }
    }
}
