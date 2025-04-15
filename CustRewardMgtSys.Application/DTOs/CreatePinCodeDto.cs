using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.DTOs
{
    public class CreatePinCodeDto
    {
        public int NumberOfPinCode { get; set; }
        public byte SubCategoryId { get; set; }
        public string BatchNo { get; set; } = string.Empty;
    }
}
