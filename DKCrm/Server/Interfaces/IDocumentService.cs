namespace DKCrm.Server.Interfaces
{
    public interface IDocumentService
    {
        Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId);
        Task<int> RemoveDocumentAsync(Guid infoSetId);
        Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId);
    }
}
