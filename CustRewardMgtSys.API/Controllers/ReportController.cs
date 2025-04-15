using CustRewardMgtSys.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CustRewardMgtSys.API.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "HeadAdmin,Admin")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private IServiceProvider _provider;
        public ReportController(IServiceProvider provider)
        {
            _provider = provider;
        }
        [HttpGet("get-dashboard-data")]
        public async Task<IActionResult> GetAllPaintCategorys()
        {
            // var userId = GetUserId();
            var paintCategoryService = _provider.GetService(typeof(IReportService)) as IReportService;
            var resultList = await paintCategoryService.GetDashboardData();
            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }
    }
}
