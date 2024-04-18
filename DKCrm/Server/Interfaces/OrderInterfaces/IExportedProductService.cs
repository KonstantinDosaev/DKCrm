using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Interfaces.OrderInterfaces
{
    public interface IExportedProductService
    {

        Task<IEnumerable<ExportedProduct>> GetAsync();
        Task<IEnumerable<ExportedProduct>> GetNotEquippedAsync(Guid productId);
        Task<IEnumerable<ExportedProduct>> GetAllNotEquippedAsync();
        Task<SortPagedResponse<ExportedProduct>> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterExportedProductTuple> request);
        Task<ExportedProduct> GetOneAsync(Guid id);
        Task<Guid> PostAsync(ExportedProduct exportedProduct);
        Task<Guid> PutAsync(ExportedProduct exportedProduct);
        Task<int> DeleteAsync(Guid id);
        Task<int> UpdateSourcesOrderItems(ExportedProduct exportedProduct);


    }
}
