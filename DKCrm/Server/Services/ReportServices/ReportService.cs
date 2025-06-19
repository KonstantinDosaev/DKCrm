using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.ReportInterfaces;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.ReportModels;
using DKCrm.Shared.Requests.ReportService;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.ReportServices;

public class ReportService : IReportService
{
    private readonly ApplicationDBContext _context;

    public ReportService(ApplicationDBContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<GetProductReportByCompanyAtChartResponse>> GetProductInPeriodByCompanyAtChartAsync(GetProductInPeriodByCompanyAtChartRequest request)
    {
        var result = new List<GetProductReportByCompanyAtChartResponse>();
        DateTime? oldsYear = null;
        if (request.TypeOrder is nameof(ExportedOrder))
        {
            foreach (var period in request.PeriodMonthsYearNumbers)
            {
                oldsYear ??= _context.ExportedOrders.Select(s => s.DateTimeUpdate).Min();
                var exportedProducts = await
                    _context.ExportedProducts.Where(w => w.Product!.PartNumber == request.PartNumber
                                                         && w.ExportedOrder.DateTimeUpdate.Value.Month == period.Month
                                                         && w.ExportedOrder.DateTimeUpdate.Value.Year == period.Year
                                                         && w.ExportedOrder.OrderIsOver == request.OrderIsOver)
                        .Include(i => i.Product)
                        .Include(exportedProduct => exportedProduct.ExportedOrder)
                        .ThenInclude(ti => ti.CompanyBuyer).ToListAsync();
                var products = exportedProducts.GroupBy(g => g.ExportedOrder.CompanyBuyerId);

                var rr = new GetProductReportByCompanyAtChartResponse()
                {
                    PeriodMonthYearNumber = period,
                    ProductsInPeriodList = new List<ProductInPeriodByCompanyAtChartDto>()
                };
                foreach (var item in products)
                {
                    var dto = new ProductInPeriodByCompanyAtChartDto()
                    {
                        CompanyId = (Guid)item.Key!,

                        ProductName = item.Select(s => s.Product.Name).FirstOrDefault(),
                        ProductPartNumber = item.Select(s => s.Product.PartNumber).FirstOrDefault(),
                        CompanyName = item.Select(s => s.ExportedOrder.CompanyBuyer.Name).FirstOrDefault(),
                        MonthYearPeriodNumber = period,
                        SumQuantity = item.Sum(s => s.Quantity)
                    };
                    rr.ProductsInPeriodList.Add(dto);
                }
                rr.OldsYears = (DateTime)oldsYear!;
                result.Add(rr);
            }
        }
        else
        {
            foreach (var period in request.PeriodMonthsYearNumbers)
            {
                oldsYear ??= _context.ImportedOrders.Select(s => s.DateTimeUpdate).Min();
                var importedProducts = await
                    _context.ImportedProducts.Where(w => w.Product!.PartNumber == request.PartNumber
                                                         && w.ImportedOrder.DateTimeUpdate.Value.Month == period.Month
                                                         && w.ImportedOrder.DateTimeUpdate.Value.Year == period.Year
                                                         && w.ImportedOrder.OrderIsOver == request.OrderIsOver)
                        .Include(i => i.Product)
                        .Include(exportedProduct => exportedProduct.ImportedOrder)
                        .ThenInclude(ti => ti.SellersCompany).ToListAsync();
                var products = importedProducts.GroupBy(g => g.ImportedOrder.SellersCompanyId);

                var rr = new GetProductReportByCompanyAtChartResponse()
                {
                    PeriodMonthYearNumber = period,
                    ProductsInPeriodList = new List<ProductInPeriodByCompanyAtChartDto>()
                };
                foreach (var item in products)
                {
                    var dto = new ProductInPeriodByCompanyAtChartDto()
                    {
                        CompanyId = (Guid)item.Key!,

                        ProductName = item.Select(s => s.Product.Name).FirstOrDefault(),
                        ProductPartNumber = item.Select(s => s.Product.PartNumber).FirstOrDefault(),
                        CompanyName = item.Select(s => s.ImportedOrder.SellersCompany.Name).FirstOrDefault(),
                        MonthYearPeriodNumber = period,
                        SumQuantity = item.Sum(s => s.Quantity)
                    };
                    rr.ProductsInPeriodList.Add(dto);
                }
                rr.OldsYears = (DateTime)oldsYear!;
                result.Add(rr);
            }
        }
        return result;
    }
    public async Task<IEnumerable<GetProductReportByCompanyAtChartResponse>> GetProductFromYearByCompanyAtChartAsync( GetProductInPeriodByCompanyAtChartRequest request)
    {
        var result = new List<GetProductReportByCompanyAtChartResponse>();
        DateTime? oldsYear = null;
        if (request.TypeOrder != null && request.TypeOrder == nameof(ExportedOrder))
        {
            foreach (var period in request.PeriodMonthsYearNumbers)
            {
                oldsYear ??= _context.ExportedOrders.Select(s => s.DateTimeUpdate).Min();
                var exportedProducts = await
                    _context.ExportedProducts.Where(w => w.Product!.PartNumber == request.PartNumber
                                                         && w.ExportedOrder!.DateTimeUpdate!.Value.Year == period.Year
                                                         && request.CompaniesIds.Contains(w.ExportedOrder!.CompanyBuyer!
                                                             .Id))
                        .Include(i => i.Product)
                        .Include(exportedProduct => exportedProduct.ExportedOrder)
                        .ThenInclude(ti => ti!.CompanyBuyer).ToListAsync();
                var products = exportedProducts.GroupBy(g => g.ExportedOrder!.CompanyBuyerId);

                var response = new GetProductReportByCompanyAtChartResponse();
                response.ProductsInPeriodList = new List<ProductInPeriodByCompanyAtChartDto>();
                response.PeriodMonthYearNumber = period;
                foreach (var item in products)
                {
                    var dto = new ProductInPeriodByCompanyAtChartDto()
                    {
                        CompanyId = (Guid)item.Key!,
                        
                        MonthYearPeriodNumber = period,
                        ProductName = item.Select(s => s.Product!.Name).FirstOrDefault()!,
                        ProductPartNumber = item.Select(s => s.Product!.PartNumber).FirstOrDefault()!,
                        CompanyName = item.Select(s => s.ExportedOrder!.CompanyBuyer!.Name).FirstOrDefault()!,
                        SumQuantity = item.Sum(s => s.Quantity)
                    };
                    response.ProductsInPeriodList.Add(dto);
                }
                response.OldsYears = (DateTime)oldsYear!;
                result.Add(response);
            }
        }
        else
        {
            foreach (var period in request.PeriodMonthsYearNumbers)
            {
                oldsYear ??= _context.ImportedOrders.Select(s => s.DateTimeUpdate).Min();
                var importedProducts = await
                    _context.ImportedProducts.Where(w => w.Product!.PartNumber == request.PartNumber
                                                         && w.ImportedOrder!.DateTimeUpdate!.Value.Year == period.Year 
                                                         && request.CompaniesIds.Contains(w.ImportedOrder!.SellersCompany!.Id))
                        .Include(i => i.Product)
                        .Include(exportedProduct => exportedProduct.ImportedOrder)
                        .ThenInclude(ti => ti!.SellersCompany).ToListAsync();
                var products = importedProducts.GroupBy(g => g.ImportedOrder!.SellersCompanyId);

                var response = new GetProductReportByCompanyAtChartResponse();
                response.ProductsInPeriodList = new List<ProductInPeriodByCompanyAtChartDto>();
                response.PeriodMonthYearNumber = period;
                foreach (var item in products)
                {
                    var dto = new ProductInPeriodByCompanyAtChartDto()
                    {
                        CompanyId = (Guid)item.Key!,
              
                        MonthYearPeriodNumber = period,
                        ProductName = item.Select(s => s.Product!.Name).FirstOrDefault()!,
                        ProductPartNumber = item.Select(s => s.Product!.PartNumber).FirstOrDefault()!,
                        CompanyName = item.Select(s => s.ImportedOrder!.SellersCompany!.Name).FirstOrDefault()!,
                        SumQuantity = item.Sum(s => s.Quantity)
                    };
                    response.ProductsInPeriodList.Add(dto);
                }
                response.OldsYears = (DateTime)oldsYear!;
                result.Add(response);
            }
        }
        return result;
    }
}