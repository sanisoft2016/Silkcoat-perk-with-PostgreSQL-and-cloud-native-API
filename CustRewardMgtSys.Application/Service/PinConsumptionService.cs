using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Domain.Enum;
using CustRewardMgtSys.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.Service
{
    public class PinConsumptionService : IPinConsumptionService
    {
        private IServiceProvider _provider;
        public PinConsumptionService(IServiceProvider provider)
        {
            _provider = provider;//Task BindPinToUser(string? userId, string requestString);
        }
        public async Task<ResponseDto> BindPinToUser(string? userId, string requestString)
        {
            var paintUserId = requestString.Split(':').ToList()[0];
            var pinCode = requestString.Split(':').ToList()[1];

            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            var isPinCodeValid = await Task.Run(() => pinCodeRepo.GetAll(x => x.Pin == pinCode, IncludeProperties: "PaintSubCategory.PaintMainCategory").FirstOrDefault());
            var now = DateTime.UtcNow;
            if (isPinCodeValid == null)
            {
                return new ResponseDto { Status = "Failure", Data = "Wrong Pin Code!" };
            }
            else if(isPinCodeValid.PinStatus == PIN_STATUS.USED || isPinCodeValid.PinStatus == PIN_STATUS.REWARDED_OF) 
            {
                return new ResponseDto { Status = "Failure", Data = "Pin Code has already been used!" };
            }
            else if (isPinCodeValid.PinStatus == PIN_STATUS.NEW)
            {
                isPinCodeValid.PinStatus = PIN_STATUS.USED;
                isPinCodeValid.DispatchedOperatedUserId = userId;
                isPinCodeValid.DateDispatched = now;
                pinCodeRepo.Update(isPinCodeValid);
                await pinCodeRepo.SaveAsync();

                var pinConsumptionRepo = _provider.GetService(typeof(IGenericRepository<PinConsumption>)) as IGenericRepository<PinConsumption>;

                var pinConsumptionObj = new PinConsumption
                {
                    PinId = isPinCodeValid.Id,
                    PainterUserId = paintUserId,
                    ConsumptionOperatedUserId = userId,
                    ConsumptionDate = now,
                };
                pinConsumptionRepo.Add(pinConsumptionObj);
                await pinConsumptionRepo.SaveAsync();

                var usageHistoryList = await GetPainterPinUsageHistoryByPainterId(paintUserId);

                return new ResponseDto { Status = "Success", Data = usageHistoryList };
            }else //if (isPinCodeValid.PinStatus == PIN_STATUS.DISPATCHED)
            {

                isPinCodeValid.PinStatus = PIN_STATUS.USED;
                pinCodeRepo.Update(isPinCodeValid);
                await pinCodeRepo.SaveAsync();
                var pinConsumptionRepo = _provider.GetService(typeof(IGenericRepository<PinConsumption>)) as IGenericRepository<PinConsumption>;
                var pinConsumptionObj = new PinConsumption
                {
                    PinId = isPinCodeValid.Id,
                    PainterUserId = paintUserId,
                    ConsumptionOperatedUserId = userId,
                    ConsumptionDate = now,
                };
                try
                {
                    pinConsumptionRepo.Add(pinConsumptionObj);
                    await pinConsumptionRepo.SaveAsync();

                    var usageHistoryList = await GetPainterPinUsageHistoryByPainterId(paintUserId);
                    return new ResponseDto { Status = "Success", Data = usageHistoryList };
                }
                catch (Exception ex)
                {
                    return new ResponseDto { Status = "Failure", Data = "Unable  to complete the process!!" };
                }
               
            }
        }
        public async Task<List<PinCode>> GetPainterPinUsageHistoryByPainterId(string pinCodeId)
        {
            var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            return await Task.Run(() => pinCodeRepo.GetAll(x => x.PinConsumption.PainterUserId == pinCodeId, IncludeProperties: "PaintSubCategory.PaintMainCategory").ToList());
        }
    }
}
