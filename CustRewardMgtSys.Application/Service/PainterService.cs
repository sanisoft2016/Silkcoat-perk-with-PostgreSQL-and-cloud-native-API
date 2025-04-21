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
    public class PainterService: IPainterService
    {
        private IServiceProvider _provider;
        public PainterService(IServiceProvider provider)
        {
            _provider = provider;
        }
        public async Task<List<PaintSubCategory>> GetAllPaintCategory()
        {
            //byte mainCategoryId = 1;
            //, 
            var itemDiscoveredRepo = _provider.GetService(typeof(IGenericRepository<PaintSubCategory>)) as IGenericRepository<PaintSubCategory>;
            return await Task.Run(() => itemDiscoveredRepo.GetAll(x => x.PaintMainCategoryId == 1, IncludeProperties: "PaintMainCategory").OrderBy(x=> x.SubCatName).ToList());
        }
    }
}
