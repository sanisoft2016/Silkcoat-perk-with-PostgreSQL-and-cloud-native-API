using CustRewardMgtSys.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustRewardMgtSys.API.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "PaintBuyer")]
    [ApiController]
    public class PainterController : ControllerBase
    {
        private IServiceProvider _provider;
        public PainterController(IServiceProvider provider)
        {
            _provider = provider;
        }
        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            return claimsIdentity.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
        }

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllPaintCategorys()
        {
           // var userId = GetUserId();
            var paintCategoryService = _provider.GetService(typeof(IPainterService)) as IPainterService;
            var resultList = await paintCategoryService.GetAllPaintCategory();
            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }

        [HttpGet("painter-pin-usage-history")]
        public async Task<IActionResult> GetPainterPinUsageHistory()
        {
            string painterId = GetUserId();
            var pinCodeConsumptionService = _provider.GetService(typeof(IPinConsumptionService)) as IPinConsumptionService;
            var resultList = await pinCodeConsumptionService.GetPainterPinUsageHistoryByPainterId(painterId);

            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }
        [HttpGet("bind-pin-to-user/{requestString}")]
        public async Task<IActionResult> BindPinToUser(string requestString)
        {
            var pinConsumptionService = _provider.GetService(typeof(IPinConsumptionService)) as IPinConsumptionService;
            var userId = GetUserId();
            requestString = userId + ":" + requestString;
            var returnObject = await pinConsumptionService.BindPinToUser(userId, requestString);
            return Ok(returnObject);
        }
    }
}
