using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.IService
{
    public interface IPinConsumptionService
    {
        Task<ResponseDto> BindPinToUser(string? userId, string requestString);
        Task<List<PinCode>> GetPainterPinUsageHistoryByPainterId(string pinCodeId);
    }
}
