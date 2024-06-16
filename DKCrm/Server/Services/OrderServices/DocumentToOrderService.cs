using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Services.OrderServices
{
    public class DocumentToOrderService : IDocumentToOrderService
    {
        private readonly ApplicationDBContext _context;

        public DocumentToOrderService(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<InfoSetFromDocumentToOrder>> GetAllDocumentsToOrder(Guid orderId)
        {
            return await _context.DocumentsToOrder.Where(w => w.OrderId == orderId).ToArrayAsync();
        }
        public async Task<int> AddDocumentToOrder(InfoSetFromDocumentToOrder infoSetFromDocumentToOrder)
        {
            _context.DocumentsToOrder.Add(infoSetFromDocumentToOrder);
            return await _context.SaveChangesAsync();
        }
    }
}
