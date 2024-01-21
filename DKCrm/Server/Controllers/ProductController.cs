using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DKCrm.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        public ProductController(ApplicationDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var t = await _context.Products.Select(s => new
            {
                s.Id,
                s.Name,
                s.ProductsInStorage,
                s.Storage,
                s.Brand,
                s.Category,
                s.PartNumber,
                s.CategoryId,
                s.BrandId,
            }).ToListAsync();

            return Ok(t);

        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await _context.Products.Select(s => new
            {
                s.Id,
                s.Name,
                s.ProductsInStorage,
                s.Storage,
                s.Brand,
                s.Category,
                s.Category!.CategoryOptions,
                s.ProductOption,
                s.PartNumber,
                s.Description,
                s.CategoryId,
                s.BrandId,
            }).FirstOrDefaultAsync(f=>f.Id==id));
        }

        [HttpPost("get-sort-filtered")]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterProductTuple> request)
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
            }).Select(s => s);
            if (request.Chapter != null && request.ChapterId != null)
            {
                switch (request.Chapter)
                {
                    case ProductFromChapterNames.Category:
                        data = data.Where(o => o.CategoryId==request.ChapterId);
                        break;
                    case ProductFromChapterNames.Brand:
                        data = data.Where(o => o.BrandId == request.ChapterId);
                        break;
                    case ProductFromChapterNames.Storage:
                        data = data.Where(o => o.Storage!.Select(s=>s.Id).Contains((Guid)request.ChapterId));
                        break;
                }
            }

            if (request.FilterTuple!=null)
            {
                if (request.FilterTuple.CategoryId!=null && request.FilterTuple.CategoryId!= Guid.Empty && request.Chapter != ProductFromChapterNames.Category)
                {
                    data = data.Where(o => o.CategoryId == request.FilterTuple.CategoryId);
                }
                if (request.FilterTuple.BrandIdList != null&& request.FilterTuple.BrandIdList.Any() && request.Chapter != ProductFromChapterNames.Brand)
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
            if (!string.IsNullOrEmpty(request.SearchString))
            {
                data = data.Where(w =>
                    w.BrandName!= null && w.BrandName.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase) ||
                    w.Name.Contains(request.SearchString) || w.PartNumber!.Contains(request.SearchString, StringComparison.InvariantCultureIgnoreCase));
            }
           
            var totalItems = data.Count();
            var sd = request.SortDirection;
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

            return Ok(new SortPagedResponse<ProductsDto>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync()});

        }
        
        [HttpGet("{searchString}")]
        public  async Task<IActionResult> GetSearch(string searchString)
        {
            return Ok(await _context.Products.Select(s => new
            {
                s.Id,
                s.Name,
                s.ProductsInStorage,
                s.Storage,
                s.Brand,
                s.Category,
                s.CategoryId,
                s.BrandId,
                s.PartNumber,
                s.Description
            }).Where(x => x.Name.ToLower().Contains(searchString.ToLower())
                          || (x.PartNumber != null && x.PartNumber.ToLower().Contains(searchString.ToLower()))).ToListAsync());
            //return Ok(await _context.Products
            //    .Include(i => i.Brand)
            //    .Include(i => i.ProductsInStorage)
            //    .Where(x => x.Name.Contains(searchString) || (x.PartNumber != null && x.PartNumber.Contains(searchString))).ToListAsync());
        }

        [HttpPost]
        public async Task<IActionResult> Post(Product product)
        {
            _context.Entry(product).State = EntityState.Added;
            if (product.ProductsInStorage != null)
            {
                foreach (var item in product.ProductsInStorage)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
            }
            await _context.SaveChangesAsync();
            return Ok(product.Id);
        }

        [HttpPut]
        public async Task<IActionResult> Put(Product product)
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
            return Ok(product);
        }
        [HttpPut("range")]
        public async Task<IActionResult> PutRange(IEnumerable<Product> products)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.Products.UpdateRange(products);
            await _context.SaveChangesAsync();
            return Ok(products.Count());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var product = await _context.Products
                .Include(i => i.ProductsInStorage).AsNoTracking()
                .Include(i => i.Storage).AsNoTracking()
                .Include(i => i.Brand).AsNoTracking()
                .Include(i => i.Category).AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id);

            _context.Entry(product!).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        [HttpPost("removerange")]
        public async Task<IActionResult> DeleteRange(IEnumerable<Guid> products)
        {
            foreach (var product in products)
            {
                _context.Entry(product!).State = EntityState.Deleted;
            }
            
            await _context.SaveChangesAsync();
            return Ok(products.Count());
        }
    }
}
