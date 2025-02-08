using DKCrm.Shared.Models.ReportModels;
using DKCrm.Shared.Requests.ReportService;

namespace DKCrm.Client.Services.ReportManager;

public interface IReportManager
{
    Task<IEnumerable<GetProductReportByCompanyAtChartResponse>> GetProductFromYearByCompanyAtChartAsync(GetProductInPeriodByCompanyAtChartRequest request);
}