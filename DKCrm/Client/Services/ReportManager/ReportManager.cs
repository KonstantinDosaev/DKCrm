using System.Net.Http.Json;
using DKCrm.Shared.Models.ReportModels;
using DKCrm.Shared.Requests.ReportService;

namespace DKCrm.Client.Services.ReportManager;

public class ReportManager : IReportManager
{
    private readonly HttpClient _httpClient;

    public ReportManager(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<GetProductReportByCompanyAtChartResponse>> GetProductFromYearByCompanyAtChartAsync(
        GetProductInPeriodByCompanyAtChartRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Report/GetProductFromYearByCompanyAtChart/", request) ?? throw new InvalidOperationException();
        return await response.Content.ReadFromJsonAsync<IEnumerable<GetProductReportByCompanyAtChartResponse>>() ?? throw new InvalidOperationException();

    }
    public async Task<IEnumerable<GetProductReportByCompanyAtChartResponse>> GetProductInPeriodByCompanyAtChartAsync(
        GetProductInPeriodByCompanyAtChartRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/Report/GetProductInPeriodByCompanyAtChart", request) ?? throw new InvalidOperationException();
        return await response.Content.ReadFromJsonAsync<IEnumerable<GetProductReportByCompanyAtChartResponse>>() ?? throw new InvalidOperationException();

    }
} 
/*public (int,int) PeriodMonthYearNumber { get; set; }
public List<ProductInPeriodByCompanyAtChartDto> ProductsInPeriodList { get; set; } = null!;*/