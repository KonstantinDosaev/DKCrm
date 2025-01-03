﻿using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Security.Claims;

namespace DKCrm.Server.Services.OrderServices
{
    public class ImportedOrderService : IImportedOrderService
    {
        private readonly ApplicationDBContext _context;

        public ImportedOrderService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ImportedOrder>> GetAsync(ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return Array.Empty<ImportedOrder>();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            await _context.Companies
                .Where(w => w.ImportedOrdersOurCompany!.Any() || w.ImportedOrdersSellersCompany!.Any())
                .Include(i => i.Employees).LoadAsync();
            return await _context.ImportedOrders.Where(w => (
                (w.SellersCompanyId != null && (!accessedComponents.Contains((Guid)w.SellersCompanyId) || (
                    accessedComponents.Contains((Guid)w.SellersCompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.SellersCompanyId).AccessUsersToComponent
                        .Contains(userId)))))).ToListAsync();
        }

        public async Task<ImportedOrder> GetDetailAsync(Guid id, ClaimsPrincipal user)
        {
            //return await _context.ImportedOrders
            //    .Include(i => i.ImportedProducts!).ThenInclude(t => t.Product).ThenInclude(t => t!.Brand).IgnoreQueryFilters()
            //    .Include(f => f.OurCompany)
            //    .Include(i => i.SellersCompany)
            //    .Include(i => i.OurEmployee)
            //    .Include(i => i.EmployeeSeller).AsSingleQuery()
            //    .FirstOrDefaultAsync(a => a.Id == id) ?? throw new InvalidOperationException();
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return new ImportedOrder();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var order = await _context.ImportedOrders.Where(w=> (
                (w.SellersCompanyId != null && (!accessedComponents.Contains((Guid)w.SellersCompanyId) || (
                    accessedComponents.Contains((Guid)w.SellersCompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.SellersCompanyId).AccessUsersToComponent
                        .Contains(userId)))))).Select(s => new ImportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                OurCompany = s.OurCompany,
                SellersCompany = s.SellersCompany,
                EmployeeSeller = s.EmployeeSeller,
                OurEmployee = s.OurEmployee,
                EmployeeSellerId = s.EmployeeSellerId,
                OurEmployeeId = s.OurEmployeeId,
               DateTimeCreated = s.DateTimeCreated,
               DateTimeUpdate = s.DateTimeUpdate,
                ImportedProducts = s.ImportedProducts,
                TransactionCurrency = s.TransactionCurrency,
                SupplierCurrency = s.SupplierCurrency,
                LocalCurrency = s.LocalCurrency,
                CurrencyPercent = s.CurrencyPercent,
                Nds = s.Nds
            }).FirstOrDefaultAsync(a => a.Id == id);
            await _context.PurchaseAtStorages.Where(w => order!.ImportedProducts!.Select(s => s.Id).Contains(w.ImportedProductId)).LoadAsync();
            await _context.PurchaseAtExports.Where(w => order!.ImportedProducts!.Select(s => s.Id).Contains(w.ImportedProductId)).LoadAsync();
            await _context.Products.Where(w => order!.ImportedProducts!.Select(s => s.ProductId).Contains(w.Id)).Include(i => i.Brand).LoadAsync();
            return order ?? throw new InvalidOperationException();
        }
        public async Task<SortPagedResponse<ImportedOrder>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderTuple> request, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return new SortPagedResponse<ImportedOrder>();
            var accessCollection = _context.AccessRestrictions;
            var accessedComponents = accessCollection.Select(s => s.AccessedComponentId);
            var data = _context.ImportedOrders.Where(w=>w.OrderIsOver == request.FilterTuple!.IsHistoryOrders).Select(s => new ImportedOrder()
            {
                Id = s.Id,
                Number = s.Number,
                OurCompany = s.OurCompany,
                OurCompanyId = s.OurCompanyId,
                SellersCompany = s.SellersCompany,
                SellersCompanyId = s.SellersCompanyId,
                EmployeeSeller = s.EmployeeSeller,
                OurEmployee = s.OurEmployee,
                EmployeeSellerId = s.EmployeeSellerId,
                OurEmployeeId = s.OurEmployeeId,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdate = s.DateTimeUpdate,
                ImportedProducts = s.ImportedProducts,
                TransactionCurrency = s.TransactionCurrency,
                SupplierCurrency = s.SupplierCurrency,
                LocalCurrency = s.LocalCurrency,
                CurrencyPercent = s.CurrencyPercent,
                Nds = s.Nds, 
                ImportedOrderStatusImportedOrders = s.ImportedOrderStatusImportedOrders,
                ImportedOrderStatus = s.ImportedOrderStatus,

            });
            data = data.Where(w => (
                (w.SellersCompanyId != null && (!accessedComponents.Contains((Guid)w.SellersCompanyId) || (
                    accessedComponents.Contains((Guid)w.SellersCompanyId)
                    && accessCollection.First(f => f.AccessedComponentId == w.SellersCompanyId).AccessUsersToComponent
                        .Contains(userId))))));
            //if (request.Chapter != null && request.ChapterId != null)
            //{
            //    data = data.Where(o => o.ImportedOrderStatusId == request.ChapterId);
            //}
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.IsOrdersWithUnreadComments == true)
                {
                    data = data.Where(wm =>
                        (_context.CommentOrders.Where(w => w.OrderId == wm.Id)
                            .Select(s => s.DateTimeUpdate).Any() && _context.LogUsersVisitToOrderComments
                            .FirstOrDefault(f => f.OrderOwnerCommentsId == wm.Id
                                                 && f.UserId == userId) == null)
                        || (_context.LogUsersVisitToOrderComments
                            .FirstOrDefault(f => f.OrderOwnerCommentsId == wm.Id
                                                 && f.UserId == userId).DateTimeVisit < _context.CommentOrders
                            .Where(w => w.OrderId == wm.Id)
                            .Select(s => s.DateTimeUpdate).Max()));
                }
                if (request.FilterTuple.CurrentStatusId != null && request.FilterTuple.CurrentStatusId != Guid.Empty)
                {
                    data = data.Where(o => o.ImportedOrderStatusImportedOrders!.OrderBy(o=>o.DateTimeCreate!.Value).LastOrDefault()!.ImportedOrderStatusId==request.FilterTuple.CurrentStatusId);
                }
                if (request.FilterTuple.CurrentPartnerCompanyId != null && request.FilterTuple.CurrentPartnerCompanyId != Guid.Empty)
                {
                    data = data.Where(o => o.SellersCompanyId == request.FilterTuple.CurrentPartnerCompanyId);
                    if (request.FilterTuple.CurrentPartnerEmployeeId != null && request.FilterTuple.CurrentPartnerEmployeeId != Guid.Empty)
                    {
                        data = data.Where(o => o.EmployeeSellerId == request.FilterTuple.CurrentPartnerEmployeeId);
                    }
                }
                if (request.FilterTuple.CurrentOurCompanyId != null && request.FilterTuple.CurrentOurCompanyId != Guid.Empty)
                {
                    data = data.Where(o => o.OurCompanyId == request.FilterTuple.CurrentOurCompanyId);
                    if (request.FilterTuple.CurrentOurEmployeeId != null && request.FilterTuple.CurrentOurEmployeeId != Guid.Empty)
                    {
                        data = data.Where(o => o.OurEmployeeId == request.FilterTuple.CurrentOurEmployeeId);
                    }
                }
                if (request.FilterTuple.CreateDateFirst != null)
                {
                    data = data.Where(w => w.DateTimeCreated!.Value.Date >= request.FilterTuple.CreateDateFirst.Value.Date);
                }
                if (request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeCreated!.Value.Date <= request.FilterTuple.CreateDateLast.Value.Date);
                }
                if (request.FilterTuple.UpdateDateFirst != null && request.FilterTuple.UpdateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeUpdate!.Value.Date >= request.FilterTuple.UpdateDateFirst.Value.Date
                                           && w.DateTimeUpdate.Value.Date <= request.FilterTuple.UpdateDateLast.Value.Date);
                }
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
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreated);
                    break;
                case "update_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdate);
                    break;
            }
         
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
   
            return new SortPagedResponse<ImportedOrder>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }
        public async Task<Guid> PostAsync(ImportedOrder importedOrder)
        {
            importedOrder.DateTimeCreated = DateTime.Now;
            var count = _context.ImportedOrders.Count(w => w.DateTimeCreated!.Value.Date == importedOrder.DateTimeCreated!.Value.Date);
            importedOrder.Number = (importedOrder.DateTimeCreated!.Value.ToString("ddMMyyyy")) + (count + 1);
           _context.Entry(importedOrder).State = EntityState.Added;
           var status = _context.ImportedOrderStatus.FirstOrDefault(f => f.Position == 0);
           if (status != null)
           {
               importedOrder.ImportedOrderStatus ??= new List<ImportedOrderStatus>();
               importedOrder.ImportedOrderStatus.Add(status);
           }
            if (importedOrder.ImportedProducts != null)
            {
                foreach (var importedProduct in importedOrder.ImportedProducts)
                {
                     _context.Entry(importedProduct).State = EntityState.Added;
                 
                   if (importedProduct.PurchaseAtExportList != null)
                       foreach (var purchaseAtExport in importedProduct.PurchaseAtExportList)
                       {
                           _context.Entry(purchaseAtExport).State = EntityState.Added;
                       }
                }
            }

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
        public async Task<int> AddStatusToOrderAsync(ImportedOrderStatusImportedOrder statusOrder)
        {
            _context.Entry(statusOrder).State = EntityState.Added;
            return await _context.SaveChangesAsync();
        }
    }
}
