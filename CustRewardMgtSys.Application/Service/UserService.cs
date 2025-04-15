using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;
using System.Net.Http;
using System.Security.Claims;
using System.Data;
using System.Runtime.CompilerServices;
using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Domain.IRepository;
using CustRewardMgtSys.Application.IService;
using Microsoft.Extensions.Configuration;
using CustRewardMgtSys.Application.DTOs;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using CustRewardMgtSys.Domain.Enum;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace CustRewardMgtSys.Application.Service
{
    public class UserService: IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private IServiceProvider _provider;
        public UserService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IServiceProvider provider)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _provider = provider;
        }

        public async Task<List<UserDto>> GetAllPaintUsers()
        {
            var userRepo = _provider.GetService(typeof(IGenericRepository<ApplicationUser>)) as IGenericRepository<ApplicationUser>;
            return await Task.Run(() => userRepo.GetAll(x=> x.UserType == USER_TYPE.PAINT_BUYER).Select(x=> new UserDto 
                { Email= x.Email, FirstName=x.FirstName, LastName=x.LastName, Gender  = x.Gender, Id = x.Id, PhoneNumber=x.PhoneNumber,
                    UserName = x.UserName, UserType = x.UserType, State = x.State, Town=x.Town}).ToList());
        }

        //private async Task<bool> ManageRolesForUserUpdate(ApplicationUser user)
        //{
        //    var hisRoleList = await _userManager.GetRolesAsync(user);
        //    await _userManager.RemoveFromRolesAsync(user, hisRoleList);
        //    await AddUserToRole(user);
        //    return true;
        //}
        //private async Task<bool> AddUserToRole(ApplicationUser userObj)
        //{
        //    if (userObj.UserType == DataAccess.Enum.USER_TYPE.SYSTEM_ADMIN)
        //    {
        //        await _userManager.AddToRoleAsync(userObj, "SystemAdministrator");
        //    }
        //    else if (userObj.UserType == DataAccess.Enum.USER_TYPE.DATA_CAPTURE_USER)
        //    {
        //        await _userManager.AddToRoleAsync(userObj, "AdminDataCapture");
        //    }
        //    else if (userObj.UserType == DataAccess.Enum.USER_TYPE.INCIDENT_REPORTER)
        //    {
        //        await _userManager.AddToRoleAsync(userObj, "IncidentReporter");
        //    }
        //    return true;
        //}
        public async Task<ResponseDto> CreateUserAsync(ApplicationUser userObj, string password)
        {
            var resultStatus = await _userManager.CreateAsync(userObj, password);
            if (!resultStatus.Succeeded)
            {
                var response = new ResponseDto
                {
                    Status = "Failure",
                    Data = "User creation failed! Please check user details and try again."
                };
                return response;
            }


            if (!await _roleManager.RoleExistsAsync("PaintBuyer"))
                await _roleManager.CreateAsync(new IdentityRole("PaintBuyer"));

            await _userManager.AddToRoleAsync(userObj, "PaintBuyer");
            var response2 = new ResponseDto
            {
                Status = "Success",
                Data = "Execution successfully completed!"
            };
            return response2;
        }
        public string GetUserNameByUserId(string userId)
        {
            return _userManager.FindByIdAsync(userId).Result.UserName;
        }

        public async Task<object> RegisterUser(UserDto userObj)
        {
            var response = new ResponseDto();
            if (userObj.Id.Trim().Length == 0 )
            { 
                var userExists = await _userManager.FindByNameAsync(userObj.UserName);
                if (userExists != null)
                {
                    response.Status = "Failure";
                    response.Data = "User already exists!";
                    return response;
                }
                var user = _userManager.Users.Where(u => u.PhoneNumber == userObj.PhoneNumber).FirstOrDefault();
                if (user != null)
                {
                    response.Status = "Failure";
                    response.Data = "Phone Number already exists!";
                    return response;
                }
                var model = new ApplicationUser();
                model.UserType = USER_TYPE.PAINT_BUYER;
                model.Gender = userObj.Gender;
                model.FirstName = userObj.FirstName;
                model.LastName = userObj.LastName;
                model.PhoneNumber = userObj.PhoneNumber;
                model.UserName = userObj.UserName;
                model.State = userObj.State;
                model.Email = userObj.Email;
                model.Town = userObj.Town;
                try
                {
                    var resultStatus = await _userManager.CreateAsync(model, userObj.Password);
                    if (!resultStatus.Succeeded)
                    {
                        var errorMsgs = "";
                        var counter = 1;
                        foreach (var item in resultStatus.Errors)
                        {
                            errorMsgs =  errorMsgs + " (" + counter + ") " + item.Description;
                            counter++;
                        }
                        response.Status = "Failure";
                        response.Data = errorMsgs;
                        return response;
                    }

                    if (!await _roleManager.RoleExistsAsync("PaintBuyer"))
                        await _roleManager.CreateAsync(new IdentityRole("PaintBuyer"));
                    await _userManager.AddToRoleAsync(model, "PaintBuyer");

                    var painterList = await GetAllPaintUsers();

                    response.Status = "Success";
                    response.Data = painterList;
                    return response;
                }
                catch (Exception ex)
                {
                    var response1 = new { Status = "Failure", Data = "Sorry unable to complete the process!" };
                    return response1;
                }
            }
            else
            {//For Update
                var userExists = await _userManager.FindByNameAsync(userObj.UserName);
                if (userExists != null)
                {
                    var user = _userManager.Users.Where(u => u.PhoneNumber == userObj.PhoneNumber).FirstOrDefault();
                    if (user != null)
                    {

                        if(user.Id != userObj.Id)
                        {
                            response.Status = "Failure";
                            response.Data = "Phone Number already exists against another user (painter)!";
                            return response;
                        }
                    }
                    userExists.Email = userObj.Email;
                    userExists.NormalizedEmail = userObj.Email.ToUpper();
                    userExists.PhoneNumber = userObj.PhoneNumber;
                    userExists.UserName = userObj.UserName;
                    userExists.NormalizedUserName = userObj.UserName.ToUpper();
                    userExists.EmailConfirmed = true;
                    userExists.UserType = userObj.UserType;
                    userExists.FirstName = userObj.FirstName;
                    userExists.LastName = userObj.LastName;
                    userExists.Gender = userObj.Gender;
                    userExists.State = userObj.State;
                    userExists.Town = userObj.Town;

                    if (userObj.Password.Trim().Length > 0)
                    {
                        if (userObj.Password.Trim().Length > 0)
                        {
                            var passHashModel = new PasswordHasher<ApplicationUser>();
                            userExists.PasswordHash = "";
                            string hashPass = passHashModel.HashPassword(userExists, userObj.Password);
                            userExists.PasswordHash = hashPass;
                        }
                    }

                   var result = await _userManager.UpdateAsync(userExists);
                    if (result.Succeeded)
                    {
                        //return result;
                        response.Status = "Success";
                        var painterList = await GetAllPaintUsers();
                        response.Data = painterList;
                        return response;
                    }
                    var errorMsgs = "";
                    var counter = 0;
                    foreach (var item in result.Errors)
                    {
                        errorMsgs = errorMsgs + " (" + counter + ") " + item.Description;
                    }
                    response.Status = "Failure";
                    response.Data = errorMsgs;
                    return response;
                }
                response.Status = "Failure";
                response.Data = "Weird!";
                return response;
            }
        }


        public async Task<object> Login(LoginDto model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.PrimarySid, user.Id)
                };
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var token = GetToken(authClaims);

                var userStatus = false;
                if (model.UserName.ToUpper() == "HeadAdmin")
                {
                    userStatus = true;
                }
                object data = new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    role = userRoles.First(),
                    isItSystemAdmin = userStatus,
                    userFullName = user.FirstName + " " + user.LastName
                };
                return new
                {
                    Status = "Success",
                    Data = data
                };
            }

            return new
            {
                Status = "Failure",
                Data = "Wrong Username or Password!"
            };
        }

        //public static string SubjectId(this ClaimsPrincipal user)
        //{ return user?.Claims?.FirstOrDefault(c => c.Type.Equals("sub", StringComparison.OrdinalIgnoreCase))?.Value; 
        //}


        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var tokenValidityInMinutes = int.Parse(_configuration["JWT:TokenValidityInMinutes"]);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        public async Task<bool> DeleteUserById(string userId)
        {
            try
            {
                var hasHeEverConsumePin = await Task.Run( () => (_provider.GetService(typeof(IGenericRepository<PaintSubCategory>)) as IGenericRepository<PinConsumption>)
                    .GetAll(c=>c.PainterUserId == userId).FirstOrDefault());
                
                if(hasHeEverConsumePin == null)
                {
                    var user = await _userManager.FindByIdAsync(userId);
                    var hisRoleList = await _userManager.GetRolesAsync(user);
                    await _userManager.RemoveFromRolesAsync(user, hisRoleList);
                    await _userManager.DeleteAsync(user);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                var pause = 0;
                return false;
            }
        }
        public async Task<bool> UploadProducts(string urlString)
        {
            var paintMainCategoryRepo = _provider.GetService(typeof(IGenericRepository<PaintMainCategory>)) as IGenericRepository<PaintMainCategory>;

            var paintCatList = new List<PaintMainCategory>
            {
                new PaintMainCategory {CatName = "Silkcoat", NoOfCharacters = 12},
                new PaintMainCategory {CatName = "Platinum",NoOfCharacters = 11},
                new PaintMainCategory {CatName = "Smart", NoOfCharacters = 10},
                new PaintMainCategory {CatName = "Icona", NoOfCharacters = 9}
            };

            foreach (var item in paintCatList)
            {
                try
                {
                    paintMainCategoryRepo.Add(item);
                    await paintMainCategoryRepo.SaveAsync();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            var paintSubCategoryRepo = _provider.GetService(typeof(IGenericRepository<PaintSubCategory>)) as IGenericRepository<PaintSubCategory>;

            using (var workbook = new XLWorkbook(urlString))
            {
                var worksheetGroups = workbook.Worksheets;
                
                foreach (var eachGroup in worksheetGroups)
                {
                    if (eachGroup.Name == "Silkcoat")
                    {
                        var counter = 0;
                        try
                        {
                            foreach (var row in eachGroup.RowsUsed().Skip(1))
                            {
                                counter++;
                                var prodName = row.Cell(1).Value.ToString().Trim();
                                var kgOrLt = row.Cell(2).Value.ToString().Trim() == "0" ||
                                                row.Cell(2).Value.ToString().Trim() == ""
                                                ? row.Cell(3).Value.ToString().Trim()
                                               : row.Cell(2).Value.ToString().Trim();

                                var coinVal = row.Cell(5).Value.ToString().Trim();
                                var productName = prodName + " :" + kgOrLt;
                                if (int.TryParse(coinVal, out int result))
                                {
                                    var doesItExist = await Task.Run(() => paintSubCategoryRepo.GetAll(x=> x.SubCatName.ToUpper() == productName.ToUpper()).FirstOrDefault());
                                    if (doesItExist == null) 
                                    {
                                        var model = new PaintSubCategory
                                        {
                                            SubCatName = productName,
                                            CoinValue = result,
                                            PaintMainCategoryId = 1
                                        };
                                        paintSubCategoryRepo.Add(model);
                                        await paintSubCategoryRepo.SaveAsync();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var check = counter;
                            throw;
                        }
                        
                    }

                    else if (eachGroup.Name == "Platinum")
                    {
                        try
                        {

                            foreach (var row in eachGroup.RowsUsed().Skip(1))
                            {
                                var prodName = row.Cell(1).Value.ToString().Trim();
                                var kgOrLt = row.Cell(2).Value.ToString().Trim() == "0" ||
                                                row.Cell(2).Value.ToString().Trim() == ""
                                                ? row.Cell(3).Value.ToString().Trim()
                                               : row.Cell(2).Value.ToString().Trim();

                                var coinVal = row.Cell(4).Value.ToString().Trim();

                                var productName =prodName + " :" + kgOrLt;
                                if (int.TryParse(coinVal, out int result))
                                {
                                    var doesItExist = await Task.Run(() => paintSubCategoryRepo.GetAll(x => x.SubCatName.ToUpper() == productName.ToUpper()).FirstOrDefault());
                                    if (doesItExist == null)
                                    {
                                        var model = new PaintSubCategory
                                        {
                                            SubCatName = productName,
                                            CoinValue = result,
                                            PaintMainCategoryId = 2
                                        };
                                        paintSubCategoryRepo.Add(model);
                                        await paintSubCategoryRepo.SaveAsync();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                    }
                    else if (eachGroup.Name == "Smart")
                    {
                        try
                        {
                            foreach (var row in eachGroup.RowsUsed().Skip(1))
                            {
                                var prodName = row.Cell(1).Value.ToString().Trim();
                                var kgOrLt = row.Cell(2).Value.ToString().Trim() == "0" ||
                                                row.Cell(2).Value.ToString().Trim() == ""
                                                ? row.Cell(3).Value.ToString().Trim()
                                               : row.Cell(2).Value.ToString().Trim();

                                var coinVal = row.Cell(4).Value.ToString().Trim();

                                var productName =prodName + " :" + kgOrLt;
                                if (int.TryParse(coinVal, out int result))
                                {
                                    var doesItExist = await Task.Run(() => paintSubCategoryRepo.GetAll(x => x.SubCatName.ToUpper() == productName.ToUpper()).FirstOrDefault());
                                    if (doesItExist == null)
                                    {
                                        var model = new PaintSubCategory
                                        {
                                            SubCatName = productName,
                                            CoinValue = result,
                                            PaintMainCategoryId = 3
                                        };
                                        paintSubCategoryRepo.Add(model);
                                        await paintSubCategoryRepo.SaveAsync();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }
                        
                    }
                    else if (eachGroup.Name == "Icona")
                    {
                        var counter = 0;
                        try
                        {
                            var iconaList = eachGroup.RowsUsed().Skip(1);
                            foreach (var row in iconaList)
                            {
                                counter++;
                                var prodName = row.Cell(1).Value.ToString().Trim();
                                var kgOrLt = row.Cell(2).Value.ToString().Trim() == "0" ||
                                                row.Cell(2).Value.ToString().Trim() == ""
                                                ? row.Cell(3).Value.ToString().Trim()
                                               : row.Cell(2).Value.ToString().Trim();

                                var coinVal = row.Cell(4).Value.ToString().Trim();

                                var productName =prodName + " :" + kgOrLt;
                                if (int.TryParse(coinVal, out int result))
                                {
                                    var doesItExist = await Task.Run(() => paintSubCategoryRepo.GetAll(x => x.SubCatName.ToUpper() == productName.ToUpper()).FirstOrDefault());
                                    if (doesItExist == null)
                                    {
                                        var model = new PaintSubCategory
                                        {
                                            SubCatName = productName,
                                            CoinValue = result,
                                            PaintMainCategoryId = 4
                                        };
                                        paintSubCategoryRepo.Add(model);
                                        await paintSubCategoryRepo.SaveAsync();
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            var check = counter++;
                            throw;
                        }
                    }
                }
            }
            return true;
        }


        public async Task<object> ChangePassword(ChangePasswordDto userObj, string userId)
        {
            ApplicationUser user2 = await _userManager.FindByIdAsync(userId);

            var passHashModel = new PasswordHasher<ApplicationUser>();
            var isPasswordValid = passHashModel.VerifyHashedPassword(user2, user2.PasswordHash, userObj.currentPassword);
            if (isPasswordValid == PasswordVerificationResult.Success)
            {
                string hashPass = passHashModel.HashPassword(user2, userObj.newPassword);
                user2.PasswordHash = hashPass;
                await _userManager.UpdateAsync(user2);

                return new
                {
                    Status = "Success",
                    Data = "Process successfully Completed!"
                };
            }
            else
            {
                return new
                {
                    Status = "Failed",
                    Data = "Wrong Old Password!"
                };
            }
        }
    }
}
