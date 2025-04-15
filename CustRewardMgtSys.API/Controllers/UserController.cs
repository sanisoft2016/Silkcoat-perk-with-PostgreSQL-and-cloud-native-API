using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Application.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace CustRewardMgtSys.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IServiceProvider _provider;
        public UserController(IServiceProvider provider)
        {
            _provider = provider;
        }

        [HttpPost]
        [Route("register-user")]
        //[Authorize(Roles = "HeadAdmin,Admin")]
        public async Task<IActionResult> RegisterUser([FromBody] UserDto userObj)
        {
            //var loggedInUserId = GetUserId();
            var userService = _provider.GetService(typeof(IUserService)) as IUserService;
            //var userName = userService.GetUserNameByUserId(loggedInUserId);

            //if (userName == "HeadAdmin")
            //{
                var response = await userService.RegisterUser(userObj);
                return Ok(response);
            //}
            //else
            //{
            //    return Ok(MainAdminUnauthorizedRequestMessageObject());
            //}
        }

        [HttpGet]
        [Route("verify-access-right")]
        [Authorize(Roles = "HeadAdmin,Admin")]
        //[AllowAnonymous]
        public IActionResult VerifyAccessRight()
        {
            //var userService = _provider.GetService(typeof(IUserService)) as IUserService;
            //string loggedInUserId = GetUserId();
            //var userName = userService.GetUserNameByUserId(loggedInUserId);
            return Ok(new { Status = "Success", Data = true });
        }
        private string GetUserId()
        {
            var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            return claimsIdentity.Claims.First(x => x.Type == ClaimTypes.PrimarySid).Value;
        }
        private object MainAdminUnauthorizedRequestMessageObject()
        {
            return new { Status = "Failed", Data = "Failed! You are not authorized to access this location." };
        }
        [HttpPost]
        [Route("post-change-password")]
        [Authorize(Roles = "HeadAdmin,PaintBuyer,Admin")]
        //[AllowAnonymous]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto userObj)
        {
            var userService = _provider.GetService(typeof(IUserService)) as IUserService;
            string userId = GetUserId();
            var response = await userService.ChangePassword(userObj, userId);
            return Ok(response);
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            var userService = _provider.GetService(typeof(IUserService)) as IUserService;
            object response = await userService.Login(model);

            //var email = HttpContext.User.FindFirstValue("email"); 
            //User.Claims.Where(x => x.Type == ClaimTypes.Email).FirstOrDefault()?.Value;
            //var claimsIdentity = (ClaimsIdentity)HttpContext.User.Identity;
            //var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            //var userId3 = claimsIdentity.Claims.First(x=> x.);

            return Ok(response);
        }

        [HttpGet("get-all-users")]
        //[Authorize(Roles = "HeadAdmin,Admin")]
        public async Task<IActionResult> GetAllAdminUsers()
        {
            var userServices = _provider.GetService(typeof(IUserService)) as IUserService;
            var resultList = await userServices.GetAllPaintUsers();
            var returnObject = new { Status = "Success", Data = resultList };
            return Ok(returnObject);
        }

        [HttpGet("delete-user-by-id/{userId}")]
        [Authorize(Roles = "HeadAdmin,Admin")]
        public async Task<IActionResult> DeleteUserById(string userId)
        {
            var userService = _provider.GetService(typeof(IUserService)) as IUserService;
            var status = await userService.DeleteUserById(userId);
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
        [RequestSizeLimit(10_000_000)]
        [HttpPost, Route("upload-products")]
        public async Task<IActionResult> CoordinateConverter([FromForm] ProductUploadDto request)
        {
            var userService = _provider.GetService(typeof(IUserService)) as IUserService;
            var file = request.ProductsFileToUpload;
            var pathToSave = "";
            var newFileName = "";

            if (file != null)
            {
                // Define the folder path to save the file
                var folderName = Path.Combine("Resources", "Products");
                pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                // Ensure the directory exists before trying to save the file
                if (!Directory.Exists(pathToSave))
                {
                    Directory.CreateDirectory(pathToSave);  // Create the directory if it doesn't exist
                }

                // Generate a new file name with the file's extension
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fileExtension = fileName.Split('.').ToList().Last();
                newFileName = Guid.NewGuid().ToString() + "." + fileExtension;

                // Define the full path where the file will be saved
                var fullPath = Path.Combine(pathToSave, newFileName);

                // Save the file to the defined path
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Ensure the copy is async
                }

                // Call the service to upload the products (or process the file)
                var response = await userService.UploadProducts(fullPath);

                // Delete the file after processing if needed
                System.IO.File.Delete(fullPath);

                // Return the response
                return Ok(new { Status = "Success", Data = response });
            }

            // Return failure if no file is uploaded
            return Ok(new { Status = "Failure", Data = false });
        }

    }
}
