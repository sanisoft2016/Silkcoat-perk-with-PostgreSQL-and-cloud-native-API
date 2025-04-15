using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Application.Service;
using CustRewardMgtSys.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Net.NetworkInformation;
using System.Security.Claims;

namespace CustRewardMgtSys.API.Controllers
{
    [Route("api/[controller]"), Authorize(Roles = "HeadAdmin,Admin")]
    [ApiController]
    public class PinCodeController : ControllerBase
    {
        private IServiceProvider _provider;
        public PinCodeController(IServiceProvider provider)
        {
            _provider = provider;
        }


        [HttpGet("get-all-pincodes")]
        public async Task<IActionResult> GetAllPinCodes()
        {
            var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
            var resultList = await pinCodeService.GetAllPinCode();
            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }

        [HttpGet("get-pinCode-by-id/{PinCodeId}")]
        public async Task<IActionResult> GetPinCodeById(byte PinCodeId)
        {
            var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
            var resultList = await pinCodeService.GetPinCodeId(PinCodeId);

            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }

        [HttpGet("delete-pinCode-by-id/{pinCodeId}")]
        public async Task<IActionResult> DeletePinCodesById(int pinCodeId)
        {
            var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
            var status = await pinCodeService.DeletePinCodeById(pinCodeId);

            if (status)
            {
                var returnObject = new { Status = "Success", Data = "Pincode has successfully been deleted!" };
                return Ok(returnObject);
            }
            
            var returnObject2 = new { Status = "Failure", Data = "The associated paint has been dispatched therefore, we need to retain the pincode" };
            return Ok(returnObject2);
        }

        [HttpPost("generate-pincode")]
        public async Task<IActionResult> CreatePinCode(CreatePinCodeDto PinCode)
        {
            var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
            var response = await pinCodeService.CreatePinCode(PinCode);
            var returnObject = new { Status = "Success", Data = response };
            return Ok(returnObject);
        }
        [HttpPost("update-pincode-status")]
        public async Task<IActionResult> UpdatePinCodeStatus(PinCodeStatusUpdateDto model)
        {
            var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
            await pinCodeService.UpdatePinCodeStatus(model);
            var returnObject = new { Status = "Success", Data = true };
            return Ok(returnObject);
        }

        //[HttpPost("mark-pincode-dispatched")]
        //public async Task<IActionResult> MarkPinCodeDisPatched(PinCodeStatusUpdateDto model)
        //{
        //    var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
        //    await pinCodeService.UpdatePinCodeStatus(model);
        //    var returnObject = new { Status = "Success", Data = true };
        //    return Ok(returnObject);
        //}

        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            return claimsIdentity.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
        }

        [HttpGet("mark-pincode-as-dispatched/{pinCodeId}")]
        public async Task<IActionResult> MarkPinCodeDisPatched(int pinCodeId)
        {
            var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
            var userId = GetUserId();
            await pinCodeService.MarkPinCodeAsDispatched(pinCodeId, userId);
            
            var returnObject = new { Status = "Success", Data = "The Status was successfully updated!" };
            return Ok(returnObject);
        }

        [HttpGet("get-pincode-details/{pinCodeId}")]
        public async Task<IActionResult> PinCodeDitails(int pinCodeId)
        {
            var pinCodeService = _provider.GetService(typeof(IPinCodeService)) as IPinCodeService;
            var response = await pinCodeService.GetPinCodeDitails(pinCodeId);

            var returnObject = new { Status = "Success", Data = response };
            return Ok(returnObject);
        }
    }
}
