using CustRewardMgtSys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.IService
{
    public interface IPainterService
    {
        Task<List<PaintSubCategory>> GetAllPaintCategory();
    }
}
