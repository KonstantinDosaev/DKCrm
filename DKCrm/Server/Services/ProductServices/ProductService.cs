using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;

namespace DKCrm.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext _context;
        public ProductService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAsync()
        {
            return await _context.Products.Select(s => new Product()
            {
                Id = s.Id,
                Name = s.Name,
                ProductsInStorage = s.ProductsInStorage,
                Storage = s.Storage,
                Brand = s.Brand,
                Category = s.Category,
                PartNumber = s.PartNumber,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
            }).ToListAsync();
        }

        public async Task<Product> GetDetailAsync(Guid id)
        {
            var product = await _context.Products.Select(s => new Product()
            {
                Id = s.Id,
                Name = s.Name,
                ProductsInStorage = s.ProductsInStorage,
                Storage = s.Storage,
                Brand = s.Brand,
                Category = s.Category,
                ProductOption = s.ProductOption,
                PartNumber = s.PartNumber,
                Description = s.Description,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
            }).FirstOrDefaultAsync(f => f.Id == id);
            if (product?.Category != null)
                await _context.Entry(product.Category).Collection(c => c.CategoryOptions!).LoadAsync();
            return product ?? throw new InvalidOperationException();
        }

        public async Task<SortPagedResponse<ProductsDto>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterProductTuple> request)
        {
            var data = _context.Products.Select(s => new ProductsDto()
            {
                Id = s.Id,
                Name = s.Name,
                ProductsInStorage = s.ProductsInStorage,
                Storage = s.Storage,
                BrandName = s.Brand!.Name,
                CategoryName = s.Category!.Name,
                PartNumber = s.PartNumber,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
                ProductOption = s.ProductOption, 
                ExportedProducts = s.ExportedProducts, 
                ImportedProducts = s.ImportedProducts,
            }).Select(s => s);
            if (request.Chapter != null && request.ChapterId != null)
            {
                switch (request.Chapter)
                {
                    case ProductFromChapterNames.Category:
                        data = data.Where(o => o.CategoryId == request.ChapterId);
                        break;
                    case ProductFromChapterNames.Brand:
                        data = data.Where(o => o.BrandId == request.ChapterId);
                        break;
                    case ProductFromChapterNames.Storage:
                        data = data.Where(o => o.Storage!.Select(s => s.Id).Contains((Guid)request.ChapterId));
                        break;
                }
            }
            if (!string.IsNullOrEmpty(request.GlobalFilterValue))
            {
                switch (request.GlobalFilterType)
                {
                    case GlobalFilterTypes.Product:
                        data = data.Where(w =>
                            w.PartNumber != null && w.PartNumber.ToLower().Contains(request.GlobalFilterValue.ToLower()));
                        break;
                    case GlobalFilterTypes.ExportedOrder:
                    {
                        var searchedListId = await _context.ExportedProducts
                            .Where(w => w.ExportedOrder != null && w.ExportedOrder.Number!.Contains(request.GlobalFilterValue))
                            .Select(s => s.ProductId).ToListAsync();
                        data = data.Where(w => searchedListId.Contains(w.Id));
                        break;
                    }
                    case GlobalFilterTypes.ImportedOrder:
                    {
                        var searchedListId = await _context.ImportedProducts
                            .Where(w => w.ImportedOrder != null && w.ImportedOrder.Number!.Contains(request.GlobalFilterValue))
                            .Select(s => s.ProductId).ToListAsync();
                        data = data.Where(w => searchedListId.Contains(w.Id));
                        break;
                    }
                    case GlobalFilterTypes.Company:
                    {
                        var searchedExportedListId = await _context.ExportedProducts
                            .Where(w => w.ExportedOrder != null && w.ExportedOrder.CompanyBuyer!.Name!.Contains(request.GlobalFilterValue))
                            .Select(s => s.ProductId).ToListAsync();
                        var searchedImportedListId = await _context.ImportedProducts
                            .Where(w => w.ImportedOrder != null && w.ImportedOrder.SellersCompany!.Name!.Contains(request.GlobalFilterValue))
                            .Select(s => s.ProductId).ToListAsync();
                        data = data.Where(w => searchedExportedListId.Contains(w.Id) 
                                               || searchedImportedListId.Contains(w.Id));
                        break;
                    }
                }
            }
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                request.SearchString = request.SearchString.Trim().ToLower();
                data = data.Where(w =>
                    w.BrandName != null && w.BrandName.ToLower().Contains(request.SearchString) ||
                    w.Name.ToLower().Contains(request.SearchString) || w.PartNumber!.ToLower().Contains(request.SearchString));
            }

            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.CategoryId != null && request.FilterTuple.CategoryId != Guid.Empty && request.Chapter != ProductFromChapterNames.Category)
                {
                    data = data.Where(o => o.CategoryId == request.FilterTuple.CategoryId);
                }
                if (request.FilterTuple.BrandIdList != null && request.FilterTuple.BrandIdList.Any() && request.Chapter != ProductFromChapterNames.Brand)
                {
                    data = data.Where(o => request.FilterTuple.BrandIdList.Contains((Guid)o.BrandId!));
                }
                if (request.FilterTuple.ProductOptions != null && request.FilterTuple.ProductOptions.Any())
                {
                    var productsId = _context.ProductOptions
                        .Where(w => request.FilterTuple.ProductOptions.Contains(w.Id))
                        .Select(o => o.ProductId).Distinct();
                    data = data.Where(w => productsId.Contains(w.Id));
                }
            }

            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "category_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.CategoryName);
                    break;
                case "partNumber_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.PartNumber);
                    break;
                case "name_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.Name);
                    break;
                case "brand_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.BrandName);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);

            return new SortPagedResponse<ProductsDto>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }

        public async Task<IEnumerable<Product>> GetSearchAsync(string searchString)
        {
            return await _context.Products.Select(s => new Product()
            {
                Id = s.Id,
                Name = s.Name,
                ProductsInStorage = s.ProductsInStorage,
                Storage = s.Storage,
                Brand = s.Brand,
                Category = s.Category,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
                PartNumber = s.PartNumber,
                Description = s.Description
            }).Where(x => x.Name.ToLower().Contains(searchString.ToLower())
                          || (x.PartNumber != null && x.PartNumber.ToLower().Contains(searchString.ToLower()))).ToListAsync();
        }

        public async Task<Guid> PostAsync(Product product)
        {
            product.DateTimeCreated = DateTime.Now;
            _context.Entry(product).State = EntityState.Added;
            if (product.ProductsInStorage != null)
            {
                foreach (var item in product.ProductsInStorage)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return product.Id;
        }

        public async Task<Guid> PutAsync(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
            if (product.ProductsInStorage != null)
            {
                var pr = await _context.ProductsInStorages.Where(w => w.ProductId == product.Id).Select(s => s.StorageId).ToListAsync();
                foreach (var item in product.ProductsInStorage)
                {
                    _context.Entry(item).State = pr.Contains(item.StorageId) ? EntityState.Modified : EntityState.Added;
                }
            }
            if (product.ProductOption != null)
            {
                var pr = await _context.ProductOptions.Where(w => w.ProductId == product.Id).Select(s => s.Id).ToListAsync();
                foreach (var item in product.ProductOption)
                {
                    _context.Entry(item).State = pr.Contains(item.Id) ? EntityState.Modified : EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return product.Id;
        }
   
        public async Task<int> PutRangeAsync(IEnumerable<Product> products)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.Products.UpdateRange(products);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var product = await _context.Products
                .Include(i => i.ProductsInStorage).AsNoTracking()
                .Include(i => i.Storage).AsNoTracking()
                .Include(i => i.Brand).AsNoTracking()
                .Include(i => i.Category).AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            _context.Entry(product!).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<Guid> products)
        {
            foreach (var product in products)
            {
                _context.Entry(product!).State = EntityState.Deleted;
            }
            return await _context.SaveChangesAsync();
        }
    }
}
