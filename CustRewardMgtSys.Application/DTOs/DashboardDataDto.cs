using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.DTOs
{
    public class DashboardDataDto
    {
        public int TotalPaintCategory { get; set; }
        public int TotalPainter { get; set; }
        public int TotalNewPinCode { get; set; }
        public int TotalOldPinCode { get; set; }
    }
}
