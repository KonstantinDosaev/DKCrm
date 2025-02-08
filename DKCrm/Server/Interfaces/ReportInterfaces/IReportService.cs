using DKCrm.Shared.Models.ReportModels;
using DKCrm.Shared.Requests.ReportService;

namespace DKCrm.Server.Interfaces.ReportInterfaces;

public interface IReportService
{
    Task<IEnumerable<GetProductReportByCompanyAtChartResponse>> GetProductInPeriodByCompanyAtChartAsync(
        GetProductInPeriodByCompanyAtChartRequest request);

    Task<IEnumerable<GetProductReportByCompanyAtChartResponse>> GetProductFromYearByCompanyAtChartAsync( GetProductInPeriodByCompanyAtChartRequest request);
}