using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.IService
{
    public interface IPinCodeService
    {
        Task<List<PinCode>> CreatePinCode(CreatePinCodeDto pinCode);
        Task<bool> DeletePinCodeById(int categoryId);
        Task<List<PinCode>> GetAllPinCode();
        Task<PinCode> GetPinCodeDitails(int pinCodeId);
        Task<List<PinCode>> GetPinCodeId(int categoryId);
        Task MarkPinCodeAsDispatched(int pinCodeId, string? userId);
        Task<object> PostPinCode(PinCode state);
        Task UpdatePinCodeStatus(PinCodeStatusUpdateDto model);
    }
}
