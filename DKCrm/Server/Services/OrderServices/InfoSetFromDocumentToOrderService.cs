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
        public async Task<InfoSetToDocument> GetOneAsync(Guid infoSetId)
        {
            return await _context.InfoSetsToDocuments.FirstOrDefaultAsync(w => w.Id == infoSetId) ?? throw new InvalidOperationException();
        }
        public async Task<IEnumerable<InfoSetToDocument>> GetAllInfoSetsDocumentsToOrderAsync(Guid orderId)
        {
            return await _context.InfoSetsToDocuments.Where(w => w.OwnerId == orderId).ToArrayAsync();
        }
        public async Task<int> AddInfoSetToOrderAsync(InfoSetToDocument infoSetToDocument)
        {
            _context.InfoSetsToDocuments.Add(infoSetToDocument);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> UpdateInfoSetToOrderAsync(InfoSetToDocument infoSetToDocument)
        {
            _context.Entry(infoSetToDocument).State = EntityState.Modified;
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
