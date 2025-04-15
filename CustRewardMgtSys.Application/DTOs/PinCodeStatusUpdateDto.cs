using CustRewardMgtSys.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.DTOs
{
    public class PinCodeStatusUpdateDto
    {
        public PIN_STATUS PinStatus { get; set; }
        public int PinCodeId { get; set; }
    }
}
