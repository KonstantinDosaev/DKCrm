using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.OrderServices
{
    public class ImportedOrderStatusService : IImportedOrderStatusService
    {
        private readonly ApplicationDBContext _context;

        public ImportedOrderStatusService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ImportedOrderStatus>> GetAsync()
        {
            var statusList = await _context.ImportedOrderStatus.Include(i => i.ImportedOrders).OrderBy(o => o.Position).ToListAsync();
            if (statusList == null || !statusList.Any())
            {
                if (Initialize())
                    statusList = await _context.ImportedOrderStatus.Include(i => i.ImportedOrders).ToListAsync();
                else
                    throw new InvalidOperationException();
            }
            return statusList;
        }

        public async Task<ImportedOrderStatus> GetDetailAsync(Guid id)
        {
            var dev = await _context.ImportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            return dev ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(ImportedOrderStatus importedOrderStatus)
        {
            _context.Add(importedOrderStatus);
            await _context.SaveChangesAsync();
            return importedOrderStatus.Id;
        }

        public async Task<Guid> PutAsync(ImportedOrderStatus importedOrderStatus)
        {
            _context.Entry(importedOrderStatus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return importedOrderStatus.Id;
        }

        public async Task<int> PutRangeAsync(IEnumerable<ImportedOrderStatus> importedOrderStatus)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ImportedOrderStatus.UpdateRange(importedOrderStatus);
            await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.ImportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return 0;
            _context.Remove(dev);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<ImportedOrderStatus> importedOrderStatus)
        {
            _context.RemoveRange(importedOrderStatus);
            return await _context.SaveChangesAsync();
        }
        public bool Initialize()
        {
            _context.ImportedOrderStatus.AddRange(new[]
            {
                new ImportedOrderStatus(){Id = Guid.NewGuid(),Position = 0,Value = ImportOrderStatusNames.BeginFormed,IsValueConstant = true},
                new ImportedOrderStatus(){Id = Guid.NewGuid(),Position = 1,Value = ImportOrderStatusNames.CompletedFormed,IsValueConstant = true},
                new ImportedOrderStatus(){Id = Guid.NewGuid(),Position = 2,Value = ImportOrderStatusNames.AwaitingConfirmation,IsValueConstant = true},
                new ImportedOrderStatus(){Id = Guid.NewGuid(),Position = 3,Value = ImportOrderStatusNames.Delivery,IsValueConstant = true},
                new ImportedOrderStatus(){Id = Guid.NewGuid(),Position = 4,Value = ImportOrderStatusNames.DeliveryCompleted,IsValueConstant = true},
                new ImportedOrderStatus(){Id = Guid.NewGuid(),Position = 5,Value = ImportOrderStatusNames.QualityTest,IsValueConstant = true},
                new ImportedOrderStatus(){Id = Guid.NewGuid(),Position = 6,Value = ImportOrderStatusNames.Completed,IsValueConstant = true}

            });
            return _context.SaveChangesAsync().IsCompletedSuccessfully;
        }

    }
}
