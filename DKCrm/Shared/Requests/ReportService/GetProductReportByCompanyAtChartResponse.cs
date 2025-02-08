using DKCrm.Shared.Models.ReportModels;

namespace DKCrm.Shared.Requests.ReportService;

public class GetProductReportByCompanyAtChartResponse
{
    public DateTime PeriodMonthYearNumber { get; set; }
    public DateTime OldsYears{get;set;}
    public List<ProductInPeriodByCompanyAtChartDto> ProductsInPeriodList { get; set; } = null!;
}