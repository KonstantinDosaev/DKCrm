using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.OrderServices
{
    public class InfoSetFromDocumentToOrderService : IInfoSetFromDocumentToOrderService
    {
        private readonly ApplicationDBContext _context;

        public InfoSetFromDocumentToOrderService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<InfoSetFromDocumentToOrder> GetOneAsync(Guid infoSetId)
        {
            return await _context.DocumentsToOrder.FirstOrDefaultAsync(w => w.Id == infoSetId) ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<InfoSetFromDocumentToOrder>> GetAllInfoSetsDocumentsToOrderAsync(Guid orderId)
        {
            return await _context.DocumentsToOrder.Where(w => w.OrderId == orderId).ToArrayAsync();
        }
        public async Task<int> AddInfoSetToOrderAsync(InfoSetFromDocumentToOrder infoSetFromDocumentToOrder)
        {
            _context.DocumentsToOrder.Add(infoSetFromDocumentToOrder);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateInfoSetToOrderAsync(InfoSetFromDocumentToOrder infoSetFromDocumentToOrder)
        {
            _context.Entry(infoSetFromDocumentToOrder).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
        public async Task<int> RemoveInfoSetFromOrderAsync(Guid id)
        {
            var item = await GetOneAsync(id);
            _context.Remove(item);
            return await _context.SaveChangesAsync();
        }
    }
}
