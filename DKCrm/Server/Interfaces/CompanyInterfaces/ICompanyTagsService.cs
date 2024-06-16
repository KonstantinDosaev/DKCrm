using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Server.Interfaces.CompanyInterfaces
{
    public interface ICompanyTagsService
    {
        Task<IEnumerable<TagsCompany>> GetAsync();
        Task<TagsCompany> GetAsync(Guid id);
        Task<Guid> PostAsync(TagsCompany tagsCompany);
        Task<Guid> PutAsync(TagsCompany tagsCompany);
        Task<int> PutRangeAsync(IEnumerable<TagsCompany> tagsCompanies);
        Task<int> DeleteAsync(Guid id);
        Task<int> DeleteRangeAsync(IEnumerable<TagsCompany> tagsCompanies);
    }
}
