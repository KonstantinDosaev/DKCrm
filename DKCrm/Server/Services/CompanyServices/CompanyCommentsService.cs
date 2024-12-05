using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DKCrm.Server.Interfaces;
using DKCrm.Shared.Models.CompanyModels;
using MudBlazor;

namespace DKCrm.Server.Services.CompanyServices
{
    public class CompanyCommentsService : ICompanyCommentsService
    {
        private readonly ApplicationDBContext _context;

        public CompanyCommentsService(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CompanyComment>> GetAllCommentsFromCompanyAsync(Guid companyId, ClaimsPrincipal user)
        {
            var comments = await _context.CompanyComments
                .Where(h => h.CompanyId == companyId)
                .OrderBy(a => a.DateTimeCreated)
                .Select(x => new CompanyComment()
                {
                    FromUserId = x.FromUserId,
                    Value = x.Value,
                    DateTimeCreated = x.DateTimeCreated,
                    Id = x.Id,
                    CompanyId = x.CompanyId,
                    Company = x.Company
                }).ToArrayAsync();
            return comments;
        }
        public async Task<SortPagedResponse<CommentOrder>> GetBySortPagedSearchAsync(SortPagedRequest<FilterOrderCommentTuple> request)
        {
            var data = _context.CommentOrders.Where(w => w.OrderId == request.FilterTuple!.OrderId).Select(x => new CommentOrder()
            {
                FromUserId = x.FromUserId,
                Value = x.Value,
                DateTimeCreated = x.DateTimeCreated,
                Id = x.Id,
                OrderId = x.OrderId,
            });

            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.CreateDateFirst != null)
                {
                    data = data.Where(w => w.DateTimeCreated! >= request.FilterTuple.CreateDateFirst.Value.Date);
                }
                if (request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeCreated! <= request.FilterTuple.CreateDateLast.Value.Date);
                }
            }

            if (!string.IsNullOrEmpty(request.SearchString))
            {
                if (request.SearchInChapter != null)
                {
                    switch (request.SearchInChapter)
                    {
                        case SearchChapterNames.TextContains:
                            data = data.Where(w =>
                                w.Value.ToLower().Contains(request.SearchString.ToLower()));
                            break;
                    }
                }
            }
            var totalItems = data.Count();

            switch (request.SortLabel)
            {
                case "create_field":
                    data = data.OrderByDirection((SortDirection)request.SortDirection!, o => o.DateTimeCreated);
                    break;
            }
            data = data.Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
            return new SortPagedResponse<CommentOrder>() { TotalItems = totalItems, Items = await data.AsSingleQuery().ToListAsync() };

        }
        public async Task<int> SaveCommentAsync(CompanyComment comment, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            if (userId == null) return 0;

            comment.FromUserId = userId;
            comment.DateTimeCreated = DateTime.UtcNow.AddHours(3);
            _context.Entry(comment).State = EntityState.Added;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var comments = _context.CompanyComments.Where(w => listId.Contains(w.Id) && w.FromUserId == userId);
            _context.CompanyComments.RemoveRange(comments);
            return await _context.SaveChangesAsync(); ;
        }
    }
}
