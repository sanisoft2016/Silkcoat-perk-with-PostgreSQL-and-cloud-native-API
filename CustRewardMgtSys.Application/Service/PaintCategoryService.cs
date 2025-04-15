using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustRewardMgtSys.Domain.IRepository;
using CustRewardMgtSys.Domain.Entities;
using CustRewardMgtSys.Application.IService;
using CustRewardMgtSys.Application.DTOs;

namespace CustRewardMgtSys.Application.Service
{
    public class PaintCategoryService : IPaintCategoryService
    {
        private IServiceProvider _provider;
        public PaintCategoryService(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<bool> DeletePaintCategoryById(byte itemDiscoveredId)
        {
            var itemDiscoveredRepo = _provider.GetService(typeof(IGenericRepository<PaintMainCategory>)) as IGenericRepository<PaintMainCategory>;
            var paintCategoryPinRepo = _provider.GetService(typeof(IGenericRepository<PinCode>)) as IGenericRepository<PinCode>;
            var paintCategoryPinObj = await Task.Run(() => paintCategoryPinRepo.GetAll(x => x.PaintSubCategoryId == itemDiscoveredId).FirstOrDefault());

            if (paintCategoryPinObj == null)
            {
                var itemDiscoveredObj = new PaintMainCategory { Id = itemDiscoveredId };
                itemDiscoveredRepo.DeleteByObject(itemDiscoveredObj);
                await itemDiscoveredRepo.SaveAsync();
                return true;
            }
            return false;
        }

        public async Task<List<PaintMainCategory>> GetPaintCategoryId(int corridorId)
        {
            var itemDiscoveredRepo = _provider.GetService(typeof(IGenericRepository<PaintMainCategory>)) as IGenericRepository<PaintMainCategory>;
            return await Task.Run(() => itemDiscoveredRepo.GetAll().ToList());
        }

        public async Task<object> PostPaintCategory(PaintMainCategory itemDiscovered)
        {
            var itemDiscoveredRepo = _provider.GetService(typeof(IGenericRepository<PaintMainCategory>)) as IGenericRepository<PaintMainCategory>;

            try
            {
                var corridorObjects = await Task.Run(() => itemDiscoveredRepo.GetAll(x => x.CatName.ToUpper() == itemDiscovered.CatName.ToUpper()).FirstOrDefault());
                if (corridorObjects == null && itemDiscovered.Id == 0)
                {
                    itemDiscoveredRepo.Add(itemDiscovered);
                    await itemDiscoveredRepo.SaveAsync();

                    var paintCategoryList = await GetAllPaintSubCategory();
                    return new { Status = "Success", Data = paintCategoryList };
                }
                else if (itemDiscovered.Id > 0)
                {
                    if ((corridorObjects == null) ||  (corridorObjects.Id == itemDiscovered.Id))
                    {
                        itemDiscoveredRepo.Update(itemDiscovered);
                        await itemDiscoveredRepo.SaveAsync();
                        var paintCategoryList = await GetAllPaintSubCategory();
                        return new { Status = "Success", Data = paintCategoryList };
                    }
                }
                return new { Status = "Failure", Data = "Failed! The paint category you have entered already exist in the system!" };
            }
            catch (Exception ex)
            {
                var pause = 0;
            }
            return new { Status = "Failure", Data = "Operation failed to complete successfully!" };
        }
        //public List<IncidentCategory> GetAllIncidenceCategories()
        //{
        //    var context = _provider.GetService(typeof(InterAtlasContext)) as InterAtlasContext;
        //    var categoryQuery = "SELECT * FROM IncidentCategories";
        //    var categoryList = context.IncidentCategories.FromSqlRaw(categoryQuery).ToListAsync().Result;
        //    return categoryList;
        //}

        //public List<DtoNameAndId> GetIncidentSubCategoriesByCategoryId(byte incidentCategoryId )
        //{
        //    var context = _provider.GetService(typeof(InterAtlasContext)) as InterAtlasContext;
        //    var categoryQuery = "SELECT Id, Name FROM IncidentSubCategories where IncidentCategoryId='" + incidentCategoryId + "'";
        //    var categoryList = context.NameAndIds.FromSqlRaw(categoryQuery).ToList();
        //    return categoryList;
        //}

        public async Task<List<PaintSubCategory>> GetAllPaintSubCategory()
        {
            var itemDiscoveredRepo = _provider.GetService(typeof(IGenericRepository<PaintSubCategory>)) as IGenericRepository<PaintSubCategory>;
            return await Task.Run(() => itemDiscoveredRepo.GetAll(IncludeProperties: "PaintMainCategory").ToList());
        }

        public async Task<List<EnumDto>> GetAllPaintCategoryForDropDown()
        {
            var itemDiscoveredRepo = _provider.GetService(typeof(IGenericRepository<PaintSubCategory>)) as IGenericRepository<PaintSubCategory>;
            return await Task.Run(() => itemDiscoveredRepo.GetAll(x=> x.PaintMainCategoryId == 2).Select(x=> new EnumDto { Id = x.Id, Name= x.SubCatName}).OrderBy(x=> x.Name).ToList());
        }
    }
}
