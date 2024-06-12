namespace DKCrm.Server.Interfaces
{
    public interface IDocumentService
    {
        void CreatePaymentInvoiceAsync(Guid orderId);
        Task<string> CreateAsync();
    }
}
