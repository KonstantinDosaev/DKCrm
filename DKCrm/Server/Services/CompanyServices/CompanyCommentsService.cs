using DKCrm.Server.Data;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DKCrm.Shared.Models.CompanyModels;
using MudBlazor;
using Microsoft.AspNetCore.Identity;
using DKCrm.Shared.Requests;
using DKCrm.Server.Interfaces.CompanyInterfaces;
using DKCrm.Server.Interfaces;
using DKCrm.Client.Pages.Authentication;
using DKCrm.Shared.Requests.OrderService;

namespace DKCrm.Server.Services.CompanyServices
{
    public class CompanyCommentsService : ICompanyCommentsService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAccessRestrictionService _accessRestrictionService;
        public CompanyCommentsService(ApplicationDBContext context, UserManager<ApplicationUser> userManager, IAccessRestrictionService accessRestrictionService)
        {
            _context = context;
            _userManager = userManager;
            _accessRestrictionService = accessRestrictionService;
        }

        public async Task<GetCommentsForPaginationResponse<CompanyComment>> GetAllCommentsFromCompanyAsync(GetCommentsForPaginationRequest request, ClaimsPrincipal user)
        {
            var comments =  _context.CompanyComments
                .Where(h => h.CompanyId == request.ComponentOwnerId)
                .OrderBy(a => a.DateTimeCreated)
                .Select(x => new CompanyComment()
                {
                    FromUserId = x.FromUserId,
                    Value = x.Value,
                    DateTimeCreated = x.DateTimeCreated,
                    Id = x.Id,
                    CompanyId = x.CompanyId, 
                    IsWarningComment = x.IsWarningComment, 
                    DateTimeUpdate = x.DateTimeUpdate
                });
            if (request.GetOnlyWarningComments == true)
                comments = comments.Where(h => h.IsWarningComment);
            
            var maxCount = comments.Count();
            comments = comments.OrderByDescending(o=>o.DateTimeUpdate).Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
            return new GetCommentsForPaginationResponse<CompanyComment>()
            {
                Items = await comments.Reverse().ToArrayAsync(), TotalItems = maxCount 
            };
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
            var commentInDb = await _context.CompanyComments.AsNoTracking().FirstOrDefaultAsync(w => w.Id == comment.Id);

            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
            if (userId == null) return 0;
            if (commentInDb == null)
            {
                comment.FromUserId = userId;
                comment.DateTimeCreated = DateTime.UtcNow.AddHours(3);
                comment.DateTimeUpdate = DateTime.UtcNow.AddHours(3);
                _context.Entry(comment).State = EntityState.Added;
            }
            else
            {
                comment.DateTimeUpdate = DateTime.UtcNow.AddHours(3);
                _context.Entry(comment).State = EntityState.Modified;
            }
            var logCurrentUserInDb = await _context.LogUsersVisitToCompanyComments.AsNoTracking()
                .FirstOrDefaultAsync(w => w.CompanyOwnerCommentsId == comment.CompanyId
                                          && w.UserId == userId) ?? null;
            if (logCurrentUserInDb != null)
            {
                logCurrentUserInDb.DateTimeVisit = DateTime.Now.AddMinutes(1); 
                _context.LogUsersVisitToCompanyComments.Entry(logCurrentUserInDb).State = EntityState.Modified;
            }
            if (comment.IsWarningComment)
            {
                var re = await  _accessRestrictionService.GetAccessFromComponentAsync(comment.CompanyId);
                List<string> users;
                if (re != null)
                {
                    users = _userManager.Users.Where(w => re.AccessUsersToComponent.Contains(w.Id))
                        .Select(s=> s.Id).ToList();
                }
                else
                {
                    users = _userManager.Users.Select(s=>s.Id).ToList();
                }
                foreach (var userAdmId in users)
                {
                    if (userAdmId == userId)
                        continue;
                    var logInDb = await _context.LogUsersVisitToCompanyComments.AsNoTracking()
                        .FirstOrDefaultAsync(w => w.CompanyOwnerCommentsId == comment.CompanyId
                                                  && w.UserId == userAdmId) ?? null;
                    if (logInDb == null)
                    {
                        _context.LogUsersVisitToCompanyComments.Entry(new LogUsersVisitToCompanyComments()
                        {
                            CompanyOwnerCommentsId = comment.CompanyId,
                            DateTimeVisit = DateTime.Now.AddMinutes(-5),
                            UserId = userAdmId
                        }).State = EntityState.Added;
                    }
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> RemoveRangeAsync(IEnumerable<Guid> listId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var comments = _context.CompanyComments.Where(w => listId.Contains(w.Id) && w.FromUserId == userId);
            _context.CompanyComments.RemoveRange(comments);
            return await _context.SaveChangesAsync(); ;
        }
        public async Task<int> SetLogUserVisit(LogUsersVisitToCompanyComments newLog, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var logInDb = await _context.LogUsersVisitToCompanyComments.AsNoTracking()
                .FirstOrDefaultAsync(w => w.CompanyOwnerCommentsId == newLog.CompanyOwnerCommentsId && w.UserId == userId) ?? null;

            if (logInDb != null)
            {
                logInDb.DateTimeVisit = newLog.DateTimeVisit;
                _context.LogUsersVisitToCompanyComments.Entry(logInDb).State = EntityState.Modified;
            }
            else
            {
                _context.LogUsersVisitToCompanyComments.Entry(newLog).State = EntityState.Added;
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<LogUsersVisitToCompanyComments> GetLogUserVisitAsync(Guid companyId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var logInDb = await _context.LogUsersVisitToCompanyComments.AsNoTracking()
                .FirstAsync(w => w.CompanyOwnerCommentsId == companyId && w.UserId == userId);

            return logInDb;
        }

        public async Task<IEnumerable<CompanyComment>> GetWarningCommentsAsync(GetWarningCommentsToCompanyRequest request,
    ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value)
                .FirstOrDefault();
           // var userRole = user.Claims.Where(a => a.Type == ClaimTypes.Role).Select(a => a.Value).FirstOrDefault();
            var comments = _context.CompanyComments.Select(x => new CompanyComment()
            {
                FromUserId = x.FromUserId,
                Value = x.Value,
                DateTimeCreated = x.DateTimeCreated,
                Id = x.Id,
                CompanyId = x.CompanyId,
                IsWarningComment = x.IsWarningComment,
                DateTimeUpdate = x.DateTimeUpdate
            }).Where(w => w.IsWarningComment == true);

            //IQueryable<Guid> orderIdList = null;
            //var commentsCompanyId = comments.Select(s => s.CompanyId);

            //   var companyIdList = userRole is RoleNames.Admin or RoleNames.SuAdmin
            //        ? _context.Companies.Where(w => commentsCompanyId.Contains(w.Id)).Select(s => s.Id)
            //        : _context.Companies.Where(w => w.Employees!.Select(s=>s.UserId).Contains(userId)
            //                                             && commentsCompanyId.Contains(w.Id)).Select(s => s.Id);
            //if (companyIdList == null)
            //    return new List<CompanyComment>();
            var logInDb =  _context.LogUsersVisitToCompanyComments;
           // if (logInDb == null || !logInDb.Any())
              //  return new List<CompanyComment>();

            if (request.GetOnlyNotOpen == true)
            {
                comments = comments.Where(w =>
                    logInDb.FirstOrDefault(f => f.CompanyOwnerCommentsId == w.CompanyId && f.UserId == userId)!
                        .DateTimeVisit < w.DateTimeUpdate);
            
            }
            else
            {
                comments = comments.Where(w => 
                    logInDb.FirstOrDefault(f => f.CompanyOwnerCommentsId == w.CompanyId && f.UserId == userId)!
                        .DateTimeVisit >= w.DateTimeUpdate );
                
            }

            return await comments.OrderBy(o => o.DateTimeUpdate).ToArrayAsync();
        }
    }
}
