using DKCrm.Server.Interfaces.ReportInterfaces;
using DKCrm.Shared.Requests.ReportService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DKCrm.Server.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
[Authorize]
public class ReportController : ControllerBase
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }
    [HttpGet]
    public async Task<IActionResult> GetProductInPeriodByCompanyAtChart(GetProductInPeriodByCompanyAtChartRequest request) 
        => Ok(await _reportService.GetProductInPeriodByCompanyAtChartAsync(request));
    
    [HttpPost]
    public async Task<IActionResult> GetProductFromYearByCompanyAtChart(  GetProductInPeriodByCompanyAtChartRequest request) 
        => Ok(await _reportService.GetProductFromYearByCompanyAtChartAsync(request));
}