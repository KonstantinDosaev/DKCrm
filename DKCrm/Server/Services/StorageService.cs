using DKCrm.Server.Data;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services
{
    public class StorageService : IStorageService
    {
        private readonly ApplicationDBContext _context;

        public StorageService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Storage>> GetAsync()
        {
            return await _context.Storages
                .Include(i => i.Address)
                .Include(i => i.Products).AsNoTracking().ToListAsync();
        }

        public async Task<Storage> GetAsync(Guid id)
        {
            var dev = await _context.Storages.AsNoTracking().FirstOrDefaultAsync(a => a.Id == id);
            return dev ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<ProductsInStorage>> GetProductInStorageListAsync(Guid productId)
        {
            //var productInStorageList = await _context.Products
            //    .Include(s => s.Storage)
            //    .Where(w => w.Id == productId)
            //    .Select(s => s.ProductsInStorage).ToListAsync();
            var productInStorageList = await _context.ProductsInStorages.Where(w=>w.ProductId == productId).ToListAsync();
            return productInStorageList;
        }
   
        public async Task<Guid> PostAsync(Storage storage)
        {
            _context.Add(storage);
            await _context.SaveChangesAsync();
            return storage.Id;
        }

        public async Task<Guid> PutAsync(Storage storage)
        {
            _context.Entry(storage).State = EntityState.Modified;
            if (storage.Address != null)
            {
                _context.Entry(storage.Address).State = storage.Address.Id != Guid.Empty ? EntityState.Modified : EntityState.Added;
            }
            await _context.SaveChangesAsync();
            return storage.Id;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.Storages.Include(i => i.Address).FirstOrDefaultAsync(a => a.Id == id);
            _context.Remove(dev!);
            _context.Remove(dev!.Address!);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> ReserveAProductAsync(SoldFromStorage soldFromStorage)
        {
            var productInStorage = _context.ProductsInStorages.FirstOrDefault(i =>
                i.ProductId == soldFromStorage.ExportedProduct!.ProductId
                && i.StorageId == soldFromStorage.StorageId);

            var soldInDb = await _context.SoldFromStorages.AsNoTracking().FirstOrDefaultAsync(f =>
                    f.ExportedProductId == soldFromStorage.ExportedProductId && f.StorageId == soldFromStorage.StorageId);

            if (productInStorage != null)
            {
                if (soldInDb != null)
                {
                    if (soldInDb.Quantity == soldFromStorage.Quantity)
                        return 0;

                    int modQuantity;
                    if (soldInDb.Quantity < soldFromStorage.Quantity)
                    {
                        modQuantity = soldFromStorage.Quantity - soldInDb.Quantity;
                        productInStorage.Quantity -= modQuantity;
                    }
                    else
                    {
                        modQuantity = soldInDb.Quantity - soldFromStorage.Quantity;
                        productInStorage.Quantity += modQuantity;
                    }
                }
                else
                {
                    productInStorage.Quantity -= soldFromStorage.Quantity;
                }

                if (productInStorage.Quantity < 0)
                    return 0;

                _context.Entry(productInStorage).State = EntityState.Modified;
            }
            else
            {
                return 0;
            }
            _context.Entry(soldFromStorage).State = soldInDb != null ? EntityState.Modified : EntityState.Added;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CancelReserveAProductAsync(SoldFromStorage soldFromStorage)
        {
            var productId = _context.ExportedProducts.FirstOrDefault(f => f.Id == soldFromStorage.ExportedProductId)!.ProductId;
            var productInStorage = _context.ProductsInStorages.FirstOrDefault(i =>
                i.ProductId == productId && i.StorageId == soldFromStorage.StorageId);
            if (productInStorage != null)
            {
                productInStorage.Quantity += soldFromStorage.Quantity;
                _context.Entry(productInStorage).State = EntityState.Modified;
            }
            else
            {
                return 0;
            }
            _context.Entry(soldFromStorage).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }
    }
}
