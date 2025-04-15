using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Domain.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CustRewardMgtSys.API.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "HeadAdmin,Admin")]
    [ApiController]
    public class PinConsumptionController : ControllerBase
    {
        private IServiceProvider _provider;
        public PinConsumptionController(IServiceProvider provider)
        {
            _provider = provider;
        }

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            return claimsIdentity.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
        }

        [HttpGet("bind-pin-to-user/{requestString}")]
        public async Task<IActionResult> BindPinToUser(string requestString)
        {
            var pinConsumptionService = _provider.GetService(typeof(IPinConsumptionService)) as IPinConsumptionService;
            var userId = GetUserId();
            var returnObject = await pinConsumptionService.BindPinToUser(userId, requestString);
            return Ok(returnObject);
        }

        [HttpGet("painter-pin-usage-history/{painterId}")]
        public async Task<IActionResult> GetPainterPinUsageHistory(string painterId)
        {
            var pinCodeConsumptionService = _provider.GetService(typeof(IPinConsumptionService)) as IPinConsumptionService;
            var resultList = await pinCodeConsumptionService.GetPainterPinUsageHistoryByPainterId(painterId);

            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }
    }
}
