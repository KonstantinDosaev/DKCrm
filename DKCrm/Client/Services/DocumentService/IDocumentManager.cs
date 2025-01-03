﻿using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;

namespace DKCrm.Client.Services.DocumentService
{
    public interface IDocumentManager
    {
        Task<IEnumerable<InfoSetFromDocumentToOrder>> GetAllInfoSetsForOrderAsync(Guid orderId);
        Task<IEnumerable<InfoSetFromDocumentToOrder>> GetOneInfoSetFromDocumentFileAsync(Guid infoSetId);
        Task<byte[]> GetDocumentBytArrayAsync(Guid infoSetId);
        Task<bool> RemoveDocumentAsync(Guid infoSetId);
         Task<bool> CreatePaymentInvoicePdfAsync(CreatePaymentInvoiceRequest request);
         Task<bool> CreateOrderSpecificationPdfAsync(CreateOrderSpecificationRequest request);
         Task<byte[]> AddStampToPdfAsync(AddStampToPdfRequest request);
         Task<bool> CreateCommercialOfferPdfAsync(CreateCommercialOfferPdfRequest request);
    }
}
