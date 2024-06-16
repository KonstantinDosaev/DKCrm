namespace DKCrm.Server.Interfaces
{
    public interface IDocumentService
    {
        Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId);
        Task<string> CreateAsync();
    }
}
