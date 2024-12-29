using DKCrm.Server.Interfaces.OrderInterfaces;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DKCrm.Server.Controllers.OrderControllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderCommentController : ControllerBase
    {
        private readonly IOrderCommentsService _orderCommentsService;

        public OrderCommentController(IOrderCommentsService orderCommentsService)
        {
            _orderCommentsService = orderCommentsService;
        }

        [HttpGet("{orderId:guid}")]
        public async Task<IActionResult> GetAllForOrder(Guid orderId) => Ok(await _orderCommentsService.GetAllCommentsFromOrderAsync(orderId, User));

        [HttpPost]
        public async Task<IActionResult> GetBySortPagedSearchChapterAsync(SortPagedRequest<FilterOrderCommentTuple> request)
            => Ok(await _orderCommentsService.GetBySortPagedSearchAsync(request));

        [HttpPost]
        public async Task<IActionResult> SaveComment(CommentOrder comment)
        {
            return Ok(await _orderCommentsService.SaveCommentAsync(comment, User));
        }

        [HttpPost]
        public async Task<IActionResult> RemoveRange(IEnumerable<Guid> listId)
        {
            return Ok(await _orderCommentsService.RemoveRangeAsync(listId, User));
        }
        [HttpPost]
        public async Task<IActionResult> SetLogUsersVisit(LogUsersVisitToOrderComments newLog)
        {
            return Ok(await _orderCommentsService.SetLogUserVisit(newLog, User));
        }
        [HttpGet("{companyId:guid}")]
        public async Task<IActionResult> GetLogUsersVisit(Guid companyId) =>
            Ok(await _orderCommentsService.GetLogUserVisitAsync(companyId, User));      
        [HttpPost]
        public async Task<IActionResult> GetWarningComments(GetWarningCommentsToOrderRequest request)
            => Ok(await _orderCommentsService.GetWarningCommentsAsync(request, User));
    }
}
