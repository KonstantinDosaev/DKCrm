using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.OrderServices
{
    public class ExportedOrderStatusService : IExportedOrderStatusServices
    {
        private readonly ApplicationDBContext _context;

        public ExportedOrderStatusService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<ExportedOrderStatus>> GetAsync()
        {
            var statusList = await _context.ExportedOrderStatus.Include(i => i.ExportedOrders).OrderBy(o => o.Position).ToListAsync();
            if (statusList == null || !statusList.Any())
            {
                if (Initialize())
                    statusList = await _context.ExportedOrderStatus.Include(i => i.ExportedOrders).ToListAsync();
                else
                    throw new InvalidOperationException();
            }
            return statusList;
        }

        public async Task<ExportedOrderStatus> GetDetailAsync(Guid id)
        {
            var status = await _context.ExportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            if (status== null)
            {
                if (Initialize())
                    status = await _context.ExportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
                else
                    throw new InvalidOperationException();
            }
            return status ?? throw new InvalidOperationException();
        }

        public async Task<Guid> PostAsync(ExportedOrderStatus exportedOrderStatus)
        {
            _context.Add(exportedOrderStatus);
            await _context.SaveChangesAsync();
            return exportedOrderStatus.Id;
        }

        public async Task<Guid> PutAsync(ExportedOrderStatus exportedOrderStatus)
        {
            _context.Entry(exportedOrderStatus).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return exportedOrderStatus.Id;
        }

        public async Task<int> PutRangeAsync(IEnumerable<ExportedOrderStatus> exportedOrderStatus)
        {
            //_context.Entry(product).State = EntityState.Modified;
            _context.ExportedOrderStatus.UpdateRange(exportedOrderStatus);
            await _context.SaveChangesAsync();
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var dev = await _context.ExportedOrderStatus.FirstOrDefaultAsync(a => a.Id == id);
            if (dev == null) return 0;

            _context.ExportedOrderStatus.Remove(dev);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<ExportedOrderStatus> status)
        {
            _context.RemoveRange(status);
            return await _context.SaveChangesAsync();
        }

        public bool Initialize()
        {
            _context.ExportedOrderStatus.AddRange(new[]
            {
                new ExportedOrderStatus(){Id = Guid.NewGuid(),Position = 0,Value = ExportOrderStatusNames.BeginFormed,IsValueConstant = true},
                new ExportedOrderStatus(){Id = Guid.NewGuid(),Position = 1,Value = ExportOrderStatusNames.ExpectComponents,IsValueConstant = true},
                new ExportedOrderStatus(){Id = Guid.NewGuid(),Position = 2,Value = ExportOrderStatusNames.Formed, IsValueConstant = true},
                new ExportedOrderStatus(){Id = Guid.NewGuid(),Position = 3,Value = ExportOrderStatusNames.OfferSentClient, IsValueConstant = true},
                new ExportedOrderStatus(){Id = Guid.NewGuid(),Position = 4,Value = ExportOrderStatusNames.OfferСonfirmedClient, IsValueConstant = true},
                new ExportedOrderStatus(){Id = Guid.NewGuid(),Position = 5,Value = ExportOrderStatusNames.Delivery, IsValueConstant = true},
                new ExportedOrderStatus(){Id = Guid.NewGuid(),Position = 6,Value = ExportOrderStatusNames.Completed, IsValueConstant = true}
            });
            return _context.SaveChangesAsync().IsCompletedSuccessfully;
        }
    }
}
