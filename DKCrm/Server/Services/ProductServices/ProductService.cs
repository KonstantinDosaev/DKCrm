using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.ProductInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.Products;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Components;
using FastExcel;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.Extensions.DependencyInjection;
using DocumentFormat.OpenXml.Drawing.Diagrams;
using System.Globalization;
using DKCrm.Server.Interfaces;
using Category = DKCrm.Shared.Models.Products.Category;
using Cell = FastExcel.Cell;
using Row = FastExcel.Row;
using Org.BouncyCastle.Asn1.Ocsp;
using DKCrm.Server.Services.ProductServices;
using DKCrm.Shared.Requests;
using DocumentFormat.OpenXml.Bibliography;

namespace DKCrm.Server.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDBContext _context;
        private readonly IConfiguration _configuration;
        private readonly IBrandService _brandService;
        private readonly ICategoryService _categoryService;
        private readonly IStorageService _storageService;
        public ProductService(ApplicationDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
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
                Price = s.Price,
                Category = s.Category,
                PartNumber = s.PartNumber,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
            }).ToListAsync();
        }

        public async Task<Product> GetDetailAsync(Guid id)
        {
            // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataQuery
            var product = await _context.Products.Select(s => new Product()
            {
                Id = s.Id,
                Name = s.Name,
                ProductsInStorage = s.ProductsInStorage,
                Storage = s.Storage,
                Brand = s.Brand,
                Price = s.Price,
                Category = s.Category,
                ProductOption = s.ProductOption,
                PartNumber = s.PartNumber,
                Description = s.Description,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
                ExportedProducts = s.ExportedProducts, 
                ImportedProducts = s.ImportedProducts, DateTimeCreated = s.DateTimeCreated,
                DateDelivery = s.DateDelivery,
            }).FirstOrDefaultAsync(f => f.Id == id);
             if (product is { Category: not null })
                 // ReSharper disable once EntityFramework.NPlusOne.IncompleteDataUsage
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
                // Brand = s.Brand,
                // Category = s.Category,
                IsDeleted = s.IsDeleted,
                DateTimeCreated = s.DateTimeCreated,
                DateTimeUpdated = s.DateTimeUpdated,
                Price = s.Price,
                PartNumber = s.PartNumber,
                CategoryId = s.CategoryId,
                BrandId = s.BrandId,
                ProductOption = s.ProductOption, 
                ExportedProducts = s.ExportedProducts, 
                ImportedProducts = s.ImportedProducts,
            }).Select(s => s);
        
                data = data.Where(o => o.IsDeleted == request.LoadOnlyDeleted);
            
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
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreated);
                    break;
                case "update_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeUpdated);
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
                Price = s.Price,
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
            product.DateTimeUpdated = DateTime.Now;
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
            product.DateTimeUpdated = DateTime.Now;
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

        public async Task<int> DeleteAsync(DeleteForGuidRequest request)
        {
            var product = await _context.Products
                .Include(i => i.ProductsInStorage).AsNoTracking()
                .Include(i => i.Storage).AsNoTracking()
                .Include(i => i.Brand).AsNoTracking()
                .Include(i => i.Category).AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.Id);
            if (product == null) return 0;

            if (request.IsFullDelete)
                product.IsFullDeleted = true;
            _context.Entry(product).State = EntityState.Deleted;
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

        public async Task<byte[]> OutputProductListToExcelAsync(List<Guid> productsIds)
        {
            var mainPath = _configuration[$"{DirectoryType.PrivateFolder}"];
            var fileName = "ExcelOutputKvota.xlsx";
            var filePath = Path.Combine(mainPath,PathsToDirectories.ProductsDocuments,fileName);
            var outputFile = new FileInfo(filePath);
            if (outputFile.Exists)
            {
                outputFile.Delete();
            }
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Лист1");

            var rowData = new List<object[]>();

            var header = SetColumns();
            rowData.Add(header);
            Product[] products;
            if (productsIds.Any())
            {
                 products = await _context.Products.Where(w => w.IsDeleted == false && productsIds.Contains(w.Id))
                    .Include(product => product.Brand).Include(product => product.Storage)
                    .Include(product => product.Category).ThenInclude(category => category.Parent)
                    .Include(product => product.ProductsInStorage!)
                    .ThenInclude(productsInStorage => productsInStorage.Storage!).ToArrayAsync();
            }
            else
            {
                products = await _context.Products.Where(w => w.IsDeleted == false)
                    .Include(product => product.Brand).Include(product => product.Storage)
                    .Include(product => product.Category).ThenInclude(category => category.Parent)
                    .Include(product => product.ProductsInStorage!)
                    .ThenInclude(productsInStorage => productsInStorage.Storage!).ToArrayAsync();
            }
            foreach (var product in products)
            {
                var row = new List<object?>
                    {product.Name,
                        product.PartNumber,
                        (string?)product.Brand?.Name,
                        product.Category?.Parent?.Name,
                        product.Category?.Name,
                        product.Description,
                        product.Price,
                        product.DayToDelivery,
                        product.SalePrice
                    };
                for (int i = 9; i < header.Length; i++)
                {
                    if (product.Storage != null && product.Storage.Select(s => s.Name).Contains(header[i]))
                    {
                        row.Add(product.ProductsInStorage!.FirstOrDefault(f => f.Storage!.Name! == (string)header[i])!.Quantity);
                    }
                    else
                    {
                        row.Add(null);
                    }
                }
                rowData.Add(row.ToArray()!);
            }
            worksheet.Cell(1, 1).InsertTable(rowData);

            worksheet.Row(1).Style.Fill.BackgroundColor = XLColor.Green;
            worksheet.Row(2).Style.Fill.BackgroundColor = XLColor.Gray;
            worksheet.Columns().AdjustToContents();
            workbook.SaveAs(outputFile.FullName);
            return await File.ReadAllBytesAsync(filePath);
        }
        private object[] SetColumns()
        {
            List<object> result = new()
            {"Наименование",
                "Парт-номер",
                "Бренд",
                "Категория",
                "Подкатегория",
                "Описание",
                "Цена",
                "Дней на доставку",
                "Скидка",
            };
            var _storage = _context.Storages.Select(s=>s.Name);
            foreach (var name in _storage)
            {
                result.Add(name);
            }
            return result.ToArray();
        }

        public async Task<List<string>> LoadProductsFromExcelAsync(byte[] excelBt)
        {
            Guid GrandCategoryIdp = default;
            List<string> resultMessage = new();
            bool _update;
           // var prodList = await GetAsync();
            var partNumbers = await _context.Products.AsNoTracking().Select(s=>s.PartNumber).ToListAsync();
            var brandList = _context.Brands.ToList();
            var categoryList = _context.Categories.ToList();
            var Storages = _context.Storages.ToList();
             List<(int,string)>? storageInTable = null;


            var products = new List<Product>();
            var productsToUpdate = new List<Product>();
            var mainPath = _configuration[$"{DirectoryType.PrivateFolder}"];
            var fileName = "ExcelOutputKvota.xlsx";
            var FilePath = Path.Combine(mainPath, PathsToDirectories.ProductsDocuments, fileName);
           
            await File.WriteAllBytesAsync(FilePath, excelBt);
            var existingFile = new FileInfo(FilePath!);
            if (!existingFile.Exists)
            {
                throw new FileNotFoundException("The file " + FilePath + " not exist");
            }
            using (var workbook = new XLWorkbook(FilePath))
            {
                var worksheet = workbook.Worksheet(1);
                var headerIsRead = false;
                var maxCellsCount = 0;
                foreach (var row in worksheet.Rows())
                {
                   
                    if (row.Cell(1).Value.ToString() == "Наименование")
                    {
                        maxCellsCount = row.Cells().Count();
                        storageInTable = new List<(int, string)>();
                        for (var i = 10; i <= row.Cells().Count(); i++)
                        {
                            storageInTable.Add((i,row.Cell(i).Value.ToString()));
                        }
                        headerIsRead = true;
                        continue;
                    }
                    if (headerIsRead == false)
                        continue;

                    var product = new Product();
                    _update = false;
                    try
                    {

                        if (!row.Cell(2).IsEmpty())
                        {
                            product.PartNumber = row.Cell(2).Value.ToString();
                        }
                        else
                            break;

                        if (partNumbers != null)
                        {
                            if (partNumbers.Contains(product.PartNumber))
                            {
                                var tempProduct = await _context.Products.AsNoTracking().Include(pr => pr.ProductsInStorage)
                                    .FirstOrDefaultAsync(f => f.PartNumber == product.PartNumber);
                                if (tempProduct != null)
                                    product = tempProduct;
                                _update = true;
                            }
                            else
                                partNumbers.Add(product.PartNumber);
                        }

                        if (!row.Cell(1).IsEmpty())
                        {
                            product.Name = row.Cell(1).Value.ToString() ?? throw new InvalidOperationException();
                        }

                        if (!row.Cell(3).IsEmpty())
                        {
                            var brand = row.Cell(3).Value.ToString().Trim();
                            var tempBrand = brandList!.Any() ? brandList!.FirstOrDefault(w => string.Equals(w.Name, brand, StringComparison.CurrentCultureIgnoreCase)) : null;
                            if (tempBrand == null)
                            {
                                tempBrand = new Brand() { Name = brand! };
                                brandList!.Add(tempBrand);
                                await _brandService.PostAsync(tempBrand);
                            }
                            product.BrandId = tempBrand!.Id;
                        }
                        if (!row.Cell(4).IsEmpty())
                        {
                            var gcategory = row.Cell(4).Value.ToString().Trim();

                            if (string.IsNullOrEmpty(gcategory))
                                continue;

                            var tempGrand = categoryList!.Any() ? categoryList!.FirstOrDefault(w => string.Equals(w.Name, gcategory, StringComparison.CurrentCultureIgnoreCase)) : null;
                            if (tempGrand == null)
                            {
                                tempGrand = new Category() { Name = gcategory };
                                categoryList!.Add(tempGrand);
                                await _categoryService.PostAsync(tempGrand);
                            }
                            else
                            {
                                if (tempGrand.Products != null && tempGrand.Products.Any())
                                    continue;

                            }
                            GrandCategoryIdp = tempGrand!.Id;
                        }
                        if (!row.Cell(5).IsEmpty())
                        {
                            var category = row.Cell(5).Value.ToString().Trim();
                            if (string.IsNullOrEmpty(category))
                                continue;

                            var tempCategory = categoryList!.Any() ? categoryList!.FirstOrDefault(w => string.Equals(w.Name, category, StringComparison.CurrentCultureIgnoreCase)) : null;
                            if (tempCategory == null)
                            {
                                tempCategory = new Category() { Name = category, ParentId = GrandCategoryIdp };
                                categoryList!.Add(tempCategory);
                                await _categoryService.PostAsync(tempCategory);
                            }
                            else
                            {
                                if (tempCategory.Children != null && tempCategory.Children.Any())
                                {
                                    var newCategory = new Category() { Id = Guid.NewGuid(), Name = category + 1, ParentId = null };
                                    categoryList!.Add(newCategory);
                                    await _categoryService.PostAsync(newCategory);
                                    product.CategoryId = newCategory!.Id;
                                    continue;
                                }
                            }

                            product.CategoryId = tempCategory!.Id;
                        }
                        if (!row.Cell(6).IsEmpty()) product.Description = row.Cell(6).Value.ToString();
                        if (!row.Cell(7).IsEmpty()) product.Price = Math.Round((Convert.ToDecimal(row.Cell(7).Value.ToString(), CultureInfo.InvariantCulture)), 2);
                        if (!row.Cell(8).IsEmpty()) product.DayToDelivery = Convert.ToInt32(Convert.ToDouble(row.Cell(8).Value.ToString(), CultureInfo.InvariantCulture));
                        if (!row.Cell(9).IsEmpty())
                        {
                            product.SalePrice = Math.Round((Convert.ToDecimal(row.Cell(9).Value.ToString(), CultureInfo.InvariantCulture)), 2);

                        }


                        for (var i = 10; i <= maxCellsCount; i++)
                        {
                            if (row.Cell(i).IsEmpty()) continue;
                            var storageInFile = storageInTable!
                                .FirstOrDefault(valueTuple => valueTuple.Item1 == i);
                            var storage = Storages.FirstOrDefault(f =>
                                f.Name == storageInFile.Item2
                                    .ToString());
                            if (storage == null)
                            {
                                resultMessage.Add($" {storageInFile.Item2} не найден");
                                continue;
                            }

                            product.ProductsInStorage ??= new List<ProductsInStorage>();

                            var productInStorage =
                                product.ProductsInStorage.FirstOrDefault(f => f.Storage == storage);
                            if (productInStorage != null)
                            {
                                productInStorage.Quantity =
                                    Convert.ToInt32(Convert.ToDouble(row.Cell(i).Value.ToString(),
                                        CultureInfo.InvariantCulture));

                            }
                            else
                            {
                       
                                    product.ProductsInStorage.Add(new ProductsInStorage()
                                    {
                                       
                                        StorageId = storage.Id,
                                        Quantity = Convert.ToInt32(Convert.ToDouble(row.Cell(i).Value.ToString(),
                                            CultureInfo.InvariantCulture))
                                    });
                                
                            }
                        }
                        if (!_update)
                        {
                            //product.Image = Prod.DefaultImageProduct;
                            product.DateTimeCreated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                            product.DateTimeUpdated = product.DateTimeCreated;
                            await PostAsync(product);
                        }
                        else
                        {
                            product.DateTimeUpdated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                            await PutAsync(product);

                        }

                    }
                    catch (Exception e)
                    {
                        resultMessage.Add($"Ошибка: проверьте правильность ячеек в продукте :---> {product.Name}");
                    }
                }
            }

           

            /*
            if (products.Count != 0)
            {
                foreach (var prodUp in products)
                {
                    await PutAsync(prodUp);
                }
            }*/

            File.Delete(FilePath);
            return resultMessage;
        }
    }
}

/*foreach (var cellItem in item.Cells.OrderBy(o => o.ColumnNumber))
{
    if (cellItem.Value != null)
    {
        if (cellItem.ColumnNumber == 1)
        {
            product.Name = cellItem.Value.ToString() ?? throw new InvalidOperationException();

            if (productsName != null && productsName.Contains(product.Name))
            {
                var tempProduct = prodList!.FirstOrDefault(w => w.Name == product.Name)!;
                product = tempProduct;
                _update = true;
            }
        }
        else if (cellItem.ColumnNumber == 2)
        {
            product.PartNumber = cellItem.Value.ToString();
        }
        else if (cellItem.ColumnNumber == 3)
        {
            var brand = cellItem.Value.ToString()!.Trim();
            var tempBrand = brandList!.Any() ? brandList!.FirstOrDefault(w => string.Equals(w.Name, brand, StringComparison.CurrentCultureIgnoreCase)) : null;
            if (tempBrand == null)
            {
                tempBrand = new Brand() { Name = brand! };
                brandList!.Add(tempBrand);
                await _brandService.PostAsync(tempBrand);
            }
            product.BrandId = tempBrand!.Id;
        }
        else if (cellItem.ColumnNumber == 4)
        {
            var gcategory = cellItem.Value.ToString()!.Trim();

            if (string.IsNullOrEmpty(gcategory))
                continue;

            var tempGrand = categoryList!.Any() ? categoryList!.FirstOrDefault(w => string.Equals(w.Name, gcategory, StringComparison.CurrentCultureIgnoreCase)) : null;
            if (tempGrand == null)
            {
                tempGrand = new Category() { Name = gcategory };
                categoryList!.Add(tempGrand);
                await _categoryService.PostAsync(tempGrand);
            }
            else
            {
                if (tempGrand.Products != null && tempGrand.Products.Any())
                    continue;

            }
            GrandCategoryIdp = tempGrand!.Id;
        }
        else if (cellItem.ColumnNumber == 5)
        {
            var category = cellItem.Value.ToString()!.Trim();
            if (string.IsNullOrEmpty(category))
                continue;

            var tempCategory = categoryList!.Any() ? categoryList!.FirstOrDefault(w => string.Equals(w.Name, category, StringComparison.CurrentCultureIgnoreCase)) : null;
            if (tempCategory == null)
            {
                tempCategory = new Category() { Name = category, ParentId = GrandCategoryIdp };
                categoryList!.Add(tempCategory);
                await _categoryService.PostAsync(tempCategory);
            }
            else
            {
                if (tempCategory.Children != null && tempCategory.Children.Any())
                {
                    var newCategory = new Category() { Id = Guid.NewGuid(), Name = category + 1, ParentId = null };
                    categoryList!.Add(newCategory);
                    await _categoryService.PostAsync(newCategory);
                    product.CategoryId = newCategory!.Id;
                    continue;
                }
            }

            product.CategoryId = tempCategory!.Id;
        }
        else if (cellItem.ColumnNumber == 6) product.Description = cellItem.Value.ToString();
        else if (cellItem.ColumnNumber == 7) product.Price = Math.Round((Convert.ToDecimal(cellItem.Value, CultureInfo.InvariantCulture)), 2);
        else if (cellItem.ColumnNumber == 8) product.DayToDelivery = Convert.ToInt32(Convert.ToDouble(cellItem.Value, CultureInfo.InvariantCulture));
        else if (cellItem.ColumnNumber == 9)
        {
            product.SalePrice = Math.Round((Convert.ToDecimal(cellItem.Value, CultureInfo.InvariantCulture)), 2);

            if (!_update)
            {
                //product.Image = Prod.DefaultImageProduct;
                product.DateTimeCreated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                product.DateTimeUpdated = product.DateTimeCreated;
                await PostAsync(product);
            }
            else
            {
                product.DateTimeUpdated = DateTime.UtcNow + new TimeSpan(0, 3, 0, 0);
                await PutAsync(product);

            }
        }
        if (cellItem.ColumnNumber >= 10)
        {
            var storage = Storages.FirstOrDefault(f =>
                f.Name == storageInTable!
                    .FirstOrDefault(f => f.ColumnNumber == cellItem.ColumnNumber)!.Value
                    .ToString());
            if (storage == null)
            {
                resultMessage.Add($"склад {storageInTable!.FirstOrDefault(f => f.ColumnNumber == cellItem.ColumnNumber)!.Value} не найден");
                continue;
            }
            product.ProductsInStorage ??= new List<ProductsInStorage>();

            var productInStorage =
                product.ProductsInStorage.FirstOrDefault(f => f.Storage == storage);
            if (productInStorage != null)
            {
                productInStorage.Quantity = Convert.ToInt32(Convert.ToDouble(cellItem.Value, CultureInfo.InvariantCulture));
                await PutAsync(product);
            }
            else
            {
                var newProduct = _context.Products.FirstOrDefault(f => f.PartNumber == product.PartNumber);
                if (newProduct != null)
                {
                    product.ProductsInStorage.Add(new ProductsInStorage()
                    {
                        ProductId = newProduct.Id,
                        StorageId = storage.Id,
                        Quantity = Convert.ToInt32(Convert.ToDouble(cellItem.Value,
                            CultureInfo.InvariantCulture))
                    });
                }
            }



        }
    }

}*/