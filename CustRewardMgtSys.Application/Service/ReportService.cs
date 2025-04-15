using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Domain.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.Service
{
    public class ReportService: IReportService
    {
        private IServiceProvider _provider;
        public ReportService(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task<DashboardDataDto> GetDashboardData()
        {
            //var pinCodeRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            var result = await Task.Run(() => (_provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>).GetAll());//.ToList()
            var totalNewPin = result.Count(x => x.PinStatus == Domain.Enum.PIN_STATUS.NEW);
            var totalUnNewPin = result.Count(x => x.PinStatus != Domain.Enum.PIN_STATUS.NEW);


            var painters = await Task.Run(() => (_provider.GetService(typeof(IGenericRepository<ApplicationUser>)) as IGenericRepository<ApplicationUser>)
                    .GetAll(x => x.UserType == Domain.Enum.USER_TYPE.PAINT_BUYER).Count());

            var paintCategories = await Task.Run(() => (_provider.GetService(typeof(IGenericRepository<PaintMainCategory>)) as IGenericRepository<PaintMainCategory>).GetAll().Count());
            return new DashboardDataDto { TotalNewPinCode = totalNewPin, TotalOldPinCode = totalUnNewPin, TotalPainter = painters, TotalPaintCategory = paintCategories };
        
        }
    }
}
