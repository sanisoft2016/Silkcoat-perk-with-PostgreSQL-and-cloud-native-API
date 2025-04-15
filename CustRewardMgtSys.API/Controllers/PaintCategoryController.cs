using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Application.Service;
using CustRewardMgtSys.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CustRewardMgtSys.API.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "HeadAdmin,Admin")]
    [ApiController]
    public class PaintCategoryController : ControllerBase
    {
        private IServiceProvider _provider;
        public PaintCategoryController(IServiceProvider provider)
        {
            _provider = provider;
        }


        //[HttpGet("get-all-incidence-categories"), Authorize(Roles = "SystemAdministrator,SuperAdministrator")]
        //public async Task<IActionResult> GetAllIncidenceCategories()
        //{
        //    var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
        //    var resultList = paintCategoryService.GetAllIncidenceCategories();
        //    var returnObject = new { Status = "Success", Data = resultList };
        //    return Ok(returnObject);
        //}


        //[HttpGet("get-category-sub-categories/{categoryId}"), Authorize(Roles = "SystemAdministrator,SuperAdministrator")]
        //public async Task<IActionResult> GetIncidenceCategorySubCategories(byte categoryId)
        //{
        //    var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
        //    var resultList = paintCategoryService.GetIncidentSubCategoriesByCategoryId(categoryId);
        //    var returnObject = new { Status = "Success", Data = resultList };
        //    return Ok(returnObject);
        //}

        //[HttpGet("get-all-action-type-categories")]
        //public async Task<IActionResult> GetAllActionTypeCategories()
        //{
        //    var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
        //    var resultList = PaintCategoryService.GetAllActionTypeCategories();
        //    var returnObject = new { Status = "Success", Data = resultList };
        //    return Ok(returnObject);
        //}

        [HttpGet("get-all-categories")]
        public async Task<IActionResult> GetAllPaintCategorys()
        {
            var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
            var resultList = await paintCategoryService.GetAllPaintSubCategory();
            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }

        [HttpGet("get-category-by-id/{PaintCategoryId}")]
        public async Task<IActionResult> GetPaintCategoryById(byte PaintCategoryId)
        {
            var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
            var resultList = await paintCategoryService.GetPaintCategoryId(PaintCategoryId);

            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }

        [HttpGet("delete-category-by-id/{PaintCategoryIdInInt}")]
        public async Task<IActionResult> DeletePaintCategorysById(byte PaintCategoryIdInInt)
        {
            var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
            var status = await paintCategoryService.DeletePaintCategoryById(PaintCategoryIdInInt);

            if (status)
            {
                var returnObject = new { Status = "Success", Data = status };
                return Ok(returnObject);
            }
            else
            {
                var returnObject = new { Status = "Failure", Data = status };
                return Ok(returnObject);
            }
        }

        [HttpPost("create-category")]
        public async Task<IActionResult> PostPaintCategory(PaintMainCategory PaintMainCategory)
        {
            var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
            var returnObject = await paintCategoryService.PostPaintCategory(PaintMainCategory);
            return Ok(returnObject);
        }

        [HttpGet("get-all-categories-for-dropdown")]
        public async Task<IActionResult> GetAllPaintCategoryForDropdown()
        {
            var paintCategoryService = _provider.GetService(typeof(IPaintCategoryService)) as IPaintCategoryService;
            var resultList = await paintCategoryService.GetAllPaintCategoryForDropDown();
            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }

    }
}
