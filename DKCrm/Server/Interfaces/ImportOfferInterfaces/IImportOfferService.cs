using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OfferModels;
using System.Security.Claims;

namespace DKCrm.Server.Interfaces.ImportOfferInterfaces
{
    public interface IImportOfferService
    {
        Task<ImportOffer> GetDetailAsync(Guid id, ClaimsPrincipal user);

        Task<SortPagedResponse<ImportOffer>> GetBySortPagedSearchChapterAsync(
            SortPagedRequest<FilterOfferTuple> request, ClaimsPrincipal user);

        Task<Guid> PostAsync(ImportOffer offer);
        Task<Guid> UpdateAsync(ImportOffer offer);
        Task<int> UpdatePriceAsync(PriceForImportOffer newPrice);
        Task<int> DeleteAsync(Guid id);
        Task<int> AddOfferToExportOrderAsync(ExportProductPriceImportOffer link);
        Task<int> AddOfferToImportProductAsync(PriceForImportOffer link);
    }
}
