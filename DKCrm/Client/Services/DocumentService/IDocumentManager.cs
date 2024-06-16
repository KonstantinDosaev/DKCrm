namespace DKCrm.Client.Services.DocumentService
{
    public interface IDocumentManager
    {
         Task<string> CreateDoc();
         Task<bool> CreatePaymentInvoicePdfAsync(Guid orderId);
    }
}
