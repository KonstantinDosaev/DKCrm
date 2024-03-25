using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DKCrm.Server.Services.OrderServices
{
    public class ImportedOrderService : IImportedOrderService
    {
        private readonly ApplicationDBContext _context;

        public ImportedOrderService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ImportedOrder>> GetAsync()
        {
            await _context.Companies
                .Where(w => w.ImportedOrdersOurCompany!.Any() || w.ImportedOrdersSellersCompany!.Any())
                .Include(i => i.Employees).LoadAsync();
            return await _context.ImportedOrders.ToListAsync();
        }

        public async Task<ImportedOrder> GetDetailAsync(Guid id)
        {
            return await _context.ImportedOrders
                .Include(i => i.ImportedProducts!).ThenInclude(t => t.Product).ThenInclude(t => t!.Brand).IgnoreQueryFilters()
                .Include(f => f.OurCompany)
                .Include(i => i.SellersCompany)
                .Include(i => i.OurEmployee)
                .Include(i => i.EmployeeSeller).AsSingleQuery()
                .FirstOrDefaultAsync(a => a.Id == id) ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ImportedOrder>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request)
        {
            var data = _context.ImportedOrders.Select(s => new ImportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                ImportedProducts = s.ImportedProducts,
                OurCompany = s.OurCompany,
                SellersCompany = s.SellersCompany,
                OurEmployee = s.OurEmployee,
                EmployeeSeller = s.EmployeeSeller,

            }).Select(s => s);
            //if (request.Chapter != null && request.ChapterId != null)
            //{
            //    data = data.Where(o => o.ImportedOrderStatusId == request.ChapterId);
            //}

            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.OurCompanies != null && request.FilterTuple.OurCompanies.Any())
                {
                    data = data.Where(o => request.FilterTuple.OurCompanies.Contains((Guid)o.OurCompanyId!));
                }
                if (request.FilterTuple.ContragentsCompanies != null && request.FilterTuple.ContragentsCompanies.Any())
                {
                    data = data.Where(o => request.FilterTuple.ContragentsCompanies.Contains((Guid)o.SellersCompanyId!));
                }
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                if (request.SearchInChapter != null)
                {
                    switch (request.SearchInChapter)
                    {
                        case SearchChapterNames.ProductPartNumber:
                            {
                                var searchedOrdersId = await _context.ImportedProducts.Where(w => w.Product!.PartNumber!.Contains(request.SearchString))
                                    .Select(s => s.ImportedOrderId).ToListAsync();
                                data = data.Where(w => searchedOrdersId.Contains(w.Id));
                                break;
                            }
                        case SearchChapterNames.CompanyName:
                            data = data.Where(w =>
                                w.OurCompany != null && w.OurCompany.Name.ToLower().Contains(request.SearchString.ToLower()) ||
                                w.SellersCompany != null && w.SellersCompany.Name.ToLower().Contains(request.SearchString.ToLower()));
                            break;
                    }
                }
            }
            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "ourCompany_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.OurCompany);
                    break;
                case "conterCompany_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.SellersCompany);
                    break;
                case "name_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Number);
                    break;
                case "number_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Id);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return new SortPagedResponse<ImportedOrder>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }
        public async Task<Guid> PostAsync(ImportedOrder importedOrder)
        {
            importedOrder.DateTimeCreated = DateTime.Now;
            var count = _context.ImportedOrders.Count(w => w.DateTimeCreated!.Value.Date == importedOrder.DateTimeCreated!.Value.Date);
            importedOrder.Number = (importedOrder.DateTimeCreated!.Value.ToShortDateString()).Replace(".", "") + (count + 1);
            _context.Entry(importedOrder).State = EntityState.Added;
            await _context.SaveChangesAsync();
            return importedOrder.Id;
        }
        public async Task<Guid> PutAsync(ImportedOrder importedOrder)
        {
           _context.Entry(importedOrder).State = EntityState.Modified;
           await _context.SaveChangesAsync();
            return importedOrder.Id;
        }
        public async Task<int> PutRangeAsync(IEnumerable<ImportedOrder> importedOrders)
        {
            foreach (var importedOrder in importedOrders)
            {
                _context.Entry(importedOrder).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.ImportedOrders.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return 0;

            _context.Remove(dev);
            await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteRangeAsync(IEnumerable<ImportedOrder> importedOrders)
        {
            _context.RemoveRange(importedOrders);
            return await _context.SaveChangesAsync();
        }
    }
}
