using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Client.Services.CompanyServices
{
    public interface ICompanyTagsManager
    {
        Task<List<TagsCompany>> GetAsync();
        Task<TagsCompany> GetDetailsAsync(Guid id);
        Task<bool> UpdateAsync(TagsCompany tagsCompany);
        Task<bool> AddAsync(TagsCompany tagsCompany);
        Task<bool> RemoveRangeAsync(IEnumerable<TagsCompany> tagsCompanies);
        Task<bool> RemoveAsync(Guid id);
    }
}
