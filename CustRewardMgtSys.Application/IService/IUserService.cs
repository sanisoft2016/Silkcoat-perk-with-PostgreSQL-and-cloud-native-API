using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.IService
{
    public interface IUserService
    {
        Task<object> RegisterUser(UserDto userObj);
        Task<object> Login(LoginDto model);
        Task<ResponseDto> CreateUserAsync(ApplicationUser user, string password);
        Task<List<UserDto>> GetAllPaintUsers();
        Task<bool> DeleteUserById(string userId);
        Task<object> ChangePassword(ChangePasswordDto userObj, string userId);
        string GetUserNameByUserId(string userId);
        Task<bool> UploadProducts(string urlString);
        Task<ResponseDto> PainterSelfRegisteration(UserDto userObj);
    }
}
