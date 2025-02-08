namespace DKCrm.Shared.Requests.ReportService;

public class GetProductInPeriodByCompanyAtChartRequest
{
    public string PartNumber { get; set; } = null!;
    public List<DateTime> PeriodMonthsYearNumbers { get; set; } = new();
    public List<Guid> CompaniesIds { get; set; } = null!;
    
    public string? TypeOrder { get; set; }
    public bool OrderIsOver { get; set; }
}