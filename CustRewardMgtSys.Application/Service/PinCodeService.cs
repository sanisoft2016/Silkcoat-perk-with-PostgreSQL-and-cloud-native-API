using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustRewardMgtSys.Domain.IRepository;
using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Domain.Tools;
using CustRewardMgtSys.Domain.Enum;
using System.Reflection;

namespace CustRewardMgtSys.Application.Service
{
    public class PinCodeService : IPinCodeService
    {
        private IServiceProvider _provider;
        public PinCodeService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<bool> DeletePinCodeById(int pinCodeId)
        {
            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            var isPinSafeToBeDeleted = await Task.Run(() => pinCodeRepo.GetAll(x => x.Id == pinCodeId && x.PinStatus == PIN_STATUS.NEW).FirstOrDefault());

            if (isPinSafeToBeDeleted != null)
            {
                var pinCodeObj = new PinCode { Id = pinCodeId };
                pinCodeRepo.DeleteByObject(pinCodeObj);
                await pinCodeRepo.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<List<PinCode>> GetPinCodeId(int pinCodeId)
        {
            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            return await Task.Run(() => pinCodeRepo.GetAll().ToList());
        }

        public async Task<object> PostPinCode(PinCode pinCode)
        {
            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;

            try
            {
                var pinCodeObjects = await Task.Run(() => pinCodeRepo.GetAll(x => x.Pin.ToUpper() == 
                        pinCode.Pin.ToUpper()).FirstOrDefault());
                if (pinCodeObjects == null && pinCode.Id == 0)
                {
                    pinCodeRepo.Add(pinCode);
                    await pinCodeRepo.SaveAsync();

                    return new { Status = "Success", Data = "Record was successfully saved!" };
                }
                return new { Status = "Failure", Data = "Failed! The paint category pin you have entered already exist in the system!" };
            }
            catch (Exception ex)
            {
                var pause = 0;
            }
            return new { Status = "Failure", Data = "Operation failed to complete successfully!" };
        }
        public async Task<List<PinCode>> GetAllPinCode()
        {
            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            return await Task.Run(() => pinCodeRepo.GetAll(IncludeProperties: "PaintSubCategory.PaintMainCategory").ToList());
        }

        public async Task MarkPinCodeAsDispatched(int pinCodeId, string userId)
        {
            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            var isPinCodeValid = await Task.Run(() => pinCodeRepo.GetAll(x => x.Id == pinCodeId).First());
            var now = DateTime.UtcNow;

            if(isPinCodeValid.PinStatus == PIN_STATUS.NEW)
            {
                isPinCodeValid.PinStatus = PIN_STATUS.DISPATCHED;
                isPinCodeValid.DispatchedOperatedUserId = userId;
                isPinCodeValid.DateDispatched = now;
                pinCodeRepo.Update(isPinCodeValid);
                await pinCodeRepo.SaveAsync();
            }
        }

        public async Task UpdatePinCodeStatus(PinCodeStatusUpdateDto model)
        {
            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>))
            as IGenericRepository<PinCode>;
            var pinCodeObj = await Task.Run(() => pinCodeRepo.GetAll(x => x.Id == model.PinCodeId).First());
            pinCodeObj.PinStatus = model.PinStatus;
            pinCodeRepo.Update(pinCodeObj);
            await pinCodeRepo.SaveAsync();
        }
        public async Task<List<PinCode>> CreatePinCode(CreatePinCodeDto pinCode)
        {
            var noOfPinToGenerate = pinCode.NumberOfPinCode;
            int surplus = (noOfPinToGenerate / 2);
            var noOfPinToGeneratePlusSurplus = noOfPinToGenerate + surplus;
            var paintCategoryObj = await Task.Run(() => (_provider.GetService(typeof(IGenericRepository<PaintSubCategory>)) as IGenericRepository<PaintSubCategory>)
                .GetAll(x => x.Id == pinCode.SubCategoryId, IncludeProperties: "PaintMainCategory").First());

            var codeLength = paintCategoryObj.PaintMainCategory.NoOfCharacters;
            var codeList = Utilities.GenerateRandomStrings(noOfPinToGeneratePlusSurplus, codeLength);

            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            var counter = 0;

            var returnList = new List<PinCode>();

            try
            {
                foreach (var item in codeList)
                {
                    if (counter == pinCode.NumberOfPinCode)
                    {
                        break;
                    }
                    

                    var isThePinExist = await Task.Run(() => pinCodeRepo.Find(x => x.Pin == item).FirstOrDefault());
                    if (isThePinExist == null)
                    {
                        var pinObj = new PinCode
                        {
                            BatchNo = pinCode.BatchNo,
                            PaintSubCategoryId = pinCode.SubCategoryId,
                            Pin = item,
                            PinStatus = PIN_STATUS.NEW,
                            GeneratedDate = DateTime.UtcNow,
                            
                        };

                        pinCodeRepo.Add(pinObj);

                        try
                        {
                            await pinCodeRepo.SaveAsync();
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                        pinObj.PaintSubCategory = paintCategoryObj;
                        returnList.Add(pinObj);
                        counter++;
                    }
                }

            }
            catch (Exception ex)
            {
                var index = counter;
                throw;
            }
            return returnList;
        }

        public async Task<PinCode> GetPinCodeDitails(int pinCodeId)
        {
            var pinCodeObj = await Task.Run(() => (_provider.GetService(typeof(IGenericRepository<PinCode>))
            as IGenericRepository<PinCode>).GetAll(x => x.Id == pinCodeId, 
                IncludeProperties: "PinConsumption,PaintSubCategory.PaintMainCategory,DispatchedOperatedUser,PinConsumption.PainterUser," +
                                    "PinConsumption.ConsumptionOperatedUser").First());
            return pinCodeObj;


        }
    }
}
