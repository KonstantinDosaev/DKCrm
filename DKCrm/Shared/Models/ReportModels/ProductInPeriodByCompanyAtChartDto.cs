namespace DKCrm.Shared.Models.ReportModels;

public class ProductInPeriodByCompanyAtChartDto
{
    public Guid ProductId { get; set; }
    public string ProductName { get; set; } = null!;
    public string ProductPartNumber { get; set; } = null!;
    public string CompanyName { get; set; } = null!;
    public Guid CompanyId { get; set; }
    public DateTime MonthYearPeriodNumber { get; set; }
    public int SumQuantity { get; set; }
    
}