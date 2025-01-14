using DKCrm.Server.Data;
using DKCrm.Server.Interfaces.OrderInterfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Constants;
using DKCrm.Shared.Models;
using MudBlazor;
using Microsoft.AspNetCore.Identity;
using DKCrm.Shared.Requests.OrderService;

namespace DKCrm.Server.Services.OrderServices
{
    public class OrderCommentsService : IOrderCommentsService
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public OrderCommentsService(ApplicationDBContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<GetCommentsForPaginationResponse<CommentOrder>> GetAllCommentsFromOrderAsync(GetCommentsForPaginationRequest request, ClaimsPrincipal user)
        {
            var comments =  _context.CommentOrders
                .Where(h => h.OrderId == request.ComponentOwnerId)
                .OrderBy(a => a.DateTimeCreated)
                .Select(x => new CommentOrder()
                {
                    FromUserId = x.FromUserId,
                    Value = x.Value,
                    DateTimeCreated = x.DateTimeCreated,
                    Id = x.Id,
                    OrderId = x.OrderId, 
                    IsWarningComment = x.IsWarningComment, 
                    OrderType = x.OrderType, 
                    DateTimeUpdate = x.DateTimeUpdate
                });
            if (request.GetOnlyWarningComments)
                comments = comments.Where(h => h.IsWarningComment);
            
            var maxCount = comments.Count();
            comments = comments.OrderByDescending(o=>o.DateTimeUpdate).Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
            return new GetCommentsForPaginationResponse<CommentOrder>()
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
                IsWarningComment = x.IsWarningComment,
                OrderType = x.OrderType
            });
   
            if (request.FilterTuple != null)
            {
                if (request.FilterTuple.CreateDateFirst != null)
                {
                    data = data.Where(w => w.DateTimeCreated >= request.FilterTuple.CreateDateFirst.Value.Date);
                }
                if (request.FilterTuple.CreateDateLast != null)
                {
                    data = data.Where(w => w.DateTimeCreated <= request.FilterTuple.CreateDateLast.Value.Date);
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
        public async Task<int> SaveCommentAsync(CommentOrder comment, ClaimsPrincipal user)
        {
            var commentInDb = await _context.CommentOrders.AsNoTracking()
                .FirstOrDefaultAsync(w => w.Id == comment.Id);
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
            await SetLogUserVisit(new LogUsersVisitToOrderComments()
            {
                OrderOwnerCommentsId = comment.OrderId,
                DateTimeVisit = DateTime.Now,
                UserId = userId
            }, user);

            if (comment.IsWarningComment)
            {
                string employeeId ="";
                if (comment.OrderType == typeof(ExportedOrder).ToString())
                {
                     employeeId = _context.ExportedOrders.
                         Include(i => i.OurEmployee)
                         .FirstOrDefault(w => w.Id == comment.OrderId)!.OurEmployee!.UserId!;
                         
                }
                else if(comment.OrderType == typeof(ImportedOrder).ToString())
                {
                    employeeId = _context.ImportedOrders.
                        Include(i=>i.OurEmployee)
                        .FirstOrDefault(w => w.Id == comment.OrderId)!.OurEmployee!.UserId!;
                }
                if (!string.IsNullOrEmpty(employeeId))
                {
                    var logInDb = await _context.LogUsersVisitToOrderComments.AsNoTracking()
                        .FirstOrDefaultAsync(w => w.OrderOwnerCommentsId == comment.OrderId 
                                                  && w.UserId == employeeId) ?? null;
                    if (logInDb == null)
                    {
                        await SetLogUserVisit(new LogUsersVisitToOrderComments()
                        {
                            OrderOwnerCommentsId = comment.OrderId,
                            DateTimeVisit = DateTime.Now,
                            UserId = employeeId
                        }, user);
                    }
                }

                var suAdmins = await _userManager.GetUsersInRoleAsync(RoleNames.SuAdmin);
                var admins = await _userManager.GetUsersInRoleAsync(RoleNames.Admin);
                var unitCollection = admins.Union(suAdmins).Select(s=>s.Id);
                foreach (var userAdmId in unitCollection)
                {
                    if (userAdmId == userId)
                        continue;
                    var logInDb = await _context.LogUsersVisitToOrderComments.AsNoTracking()
                        .FirstOrDefaultAsync(w => w.OrderOwnerCommentsId == comment.OrderId
                                                  && w.UserId == employeeId) ?? null;
                    if (logInDb == null)
                    {
                        _context.LogUsersVisitToOrderComments.Entry(new LogUsersVisitToOrderComments()
                        {
                            OrderOwnerCommentsId = comment.OrderId,
                            DateTimeVisit = DateTime.Now,
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
            var comments = _context.CommentOrders.Where(w => listId.Contains(w.Id) && w.FromUserId == userId);
            var count = comments.Count();
            _context.CommentOrders.RemoveRange(comments);
            await _context.SaveChangesAsync();
            return count;
        }
        public async Task<int> SetLogUserVisit(LogUsersVisitToOrderComments newLog, ClaimsPrincipal user)
        {
            //var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var logInDb = await _context.LogUsersVisitToOrderComments.AsNoTracking()
                .FirstOrDefaultAsync(w => w.OrderOwnerCommentsId == newLog.OrderOwnerCommentsId && w.UserId == newLog.UserId) ?? null;
            if (logInDb != null )
            {
                logInDb.DateTimeVisit = newLog.DateTimeVisit;
                _context.LogUsersVisitToOrderComments.Entry(logInDb).State = EntityState.Modified;
            }
            else
            {
                _context.LogUsersVisitToOrderComments.Entry(newLog).State = EntityState.Added;
            }
          
            return await _context.SaveChangesAsync();
        }
        public async Task<LogUsersVisitToOrderComments> GetLogUserVisitAsync(Guid orderId, ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .Select(a => a.Value).FirstOrDefault();
            var logInDb = await _context.LogUsersVisitToOrderComments.AsNoTracking()
                .FirstAsync(w => w.OrderOwnerCommentsId == orderId && w.UserId == userId);

            return logInDb;
        }

        public async Task<GetCommentsForPaginationResponse<CommentOrder>> GetWarningCommentsAsync(GetWarningCommentsToOrderRequest request,
            ClaimsPrincipal user)
        {
            var userId = user.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier)
                .Select(a => a.Value).FirstOrDefault();
            var userRole = user.Claims.Where(a => a.Type == ClaimTypes.Role)
                .Select(a => a.Value).FirstOrDefault();
            var comments = _context.CommentOrders.Select(x => new CommentOrder()
                {
                    FromUserId = x.FromUserId,
                    Value = x.Value,
                    DateTimeCreated = x.DateTimeCreated,
                    Id = x.Id,
                    OrderId = x.OrderId,
                    IsWarningComment = x.IsWarningComment,
                    OrderType = x.OrderType,
                    DateTimeUpdate = x.DateTimeUpdate
                })
                .Where(w => w.IsWarningComment == true);

            IQueryable<Guid>? orderIdList = null;
            var commentsOrderId = comments.Select(s => s.OrderId);
            if (request.OrderType == typeof(ExportedOrder).ToString())
            {
                orderIdList = userRole is RoleNames.Admin or RoleNames.SuAdmin
                    ? _context.ExportedOrders.Where(w => commentsOrderId.Contains(w.Id)).Select(s => s.Id)
                    : _context.ExportedOrders.Where(w => w.OurEmployee!.UserId == userId 
                                                         && commentsOrderId.Contains(w.Id)).Select(s => s.Id);

            }
            if (request.OrderType == typeof(ImportedOrder).ToString())
            {
                orderIdList = userRole is RoleNames.Admin or RoleNames.SuAdmin
                    ? _context.ImportedOrders.Where(w => commentsOrderId.Contains(w.Id)).Select(s => s.Id)
                    : _context.ImportedOrders.Where(w => w.OurEmployee!.UserId == userId
                                                         && commentsOrderId.Contains(w.Id))
                        .Select(s => s.Id);
            }
            if (orderIdList == null || !orderIdList.Any())
                return new GetCommentsForPaginationResponse<CommentOrder>()
                {
                    Items = new List<CommentOrder>(), TotalItems = 0 
                };
            if (request.GetOnlyNotOpen)
            {
                
                var logInDb = _context.LogUsersVisitToOrderComments
                    .Where(w => w.UserId == userId && orderIdList.Contains(w.OrderOwnerCommentsId));

                comments = comments.Where(w =>logInDb.FirstOrDefault(f => f.OrderOwnerCommentsId == w.OrderId)!
                                                   .DateTimeVisit < w.DateTimeUpdate);
            }
            else
            {
                var logInDb = _context.LogUsersVisitToOrderComments
                    .Where(w => w.UserId == userId && orderIdList.Contains(w.OrderOwnerCommentsId));

                comments = comments.Where(w => logInDb.FirstOrDefault(f => f.OrderOwnerCommentsId == w.OrderId)!
                    .DateTimeVisit > w.DateTimeUpdate);
            }
            var maxCount = comments.Count();
            comments = comments.OrderByDescending(o=>o.DateTimeUpdate).Skip(request.PageIndex * request.PageSize).Take(request.PageSize);
            return new GetCommentsForPaginationResponse<CommentOrder>()
            {
                Items = await comments.Reverse().ToArrayAsync(), TotalItems = maxCount 
            };
        }
    }
}
