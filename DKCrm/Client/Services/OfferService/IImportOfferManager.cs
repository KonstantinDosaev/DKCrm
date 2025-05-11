using DKCrm.Shared.Models.OfferModels;
using DKCrm.Shared.Models;


namespace DKCrm.Client.Services.OfferService
{
    public interface IImportOfferManager
    {
        Task<ImportOffer> GetDetailsAsync(Guid id);
        Task<SortPagedResponse<ImportOffer>> GetBySortFilterPaginationAsync(SortPagedRequest<FilterOfferTuple> request,
            CancellationToken token);
        Task<bool> AddAsync(ImportOffer item);
        Task<bool> UpdateAsync(ImportOffer item);
        Task<bool> UpdatePriceAsync(PriceForImportOffer item);
        Task<bool> AddOfferToExportOrderAsync(ExportProductPriceImportOffer link);
        Task<bool> AddOfferToImportOrderAsync(ImportProductPriceImportOffer link);
    }
}
