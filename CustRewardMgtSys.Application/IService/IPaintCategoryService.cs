using CustRewardMgtSys.Application.DTOs;
using CustRewardMgtSys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustRewardMgtSys.Application.IService
{
    public interface IPaintCategoryService
    {
        Task<bool> DeletePaintCategoryById(byte categoryId); 
        Task<List<PaintSubCategory>> GetAllPaintSubCategory();
        Task<List<PaintMainCategory>> GetPaintCategoryId(int categoryId);
        Task<object> PostPaintCategory(PaintMainCategory state);
        Task<List<EnumDto>> GetAllPaintCategoryForDropDown();
    }
}
