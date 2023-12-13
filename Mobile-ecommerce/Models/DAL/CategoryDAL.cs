using Mobile_ecommerce.Areas.Admin.Model;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.Category;
using Mobile_ecommerce.Models.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mobile_ecommerce.Models.DAL
{
    public class CategoryDAL
    {
        private MobileDbContext dbContext;
        public CategoryDAL()
        {
            dbContext = new MobileDbContext();
        }
        public async Task<int> Create(DetailCate pro)
        {
            try
            {
                var check = await dbContext.Categories.Where(x => x.CategoryID.Equals(pro.LoaiID)).FirstOrDefaultAsync();
                if (check != null) return 0;
                else
                {
                    var type = new Category()
                    {                        
                        CategoryName=pro.TenLoai,
                    };
                    dbContext.Categories.Add(type);
                    var result = await dbContext.SaveChangesAsync();
                    return result;
                }
            }
            catch
            {
                return -1;
            }
        }
        public async Task<bool> Update(DetailCate edit)
        {
            try
            {
                var pro = await dbContext.Categories.FindAsync(edit.LoaiID);
                pro.CategoryID = edit.LoaiID;
                pro.CategoryName = edit.TenLoai;
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {             
                return false;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var pro = await dbContext.Categories.FindAsync(id);
                if (pro == null) return false;
                dbContext.Categories.Remove(pro);
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<ResultPaging<DetailCate>> GetList(GetListPaging paging)
        {
            var model = from a in dbContext.Categories
                        select new DetailCate()
                        {
                           LoaiID=a.CategoryID,
                           TenLoai=a.CategoryName,
                        };
            if (!string.IsNullOrEmpty(paging.keyWord))
            {
                model = model.Where(x => x.TenLoai.Contains(paging.keyWord.Trim()));
            }

            int total = await model.CountAsync();

            var items = await model.OrderBy(x => x.LoaiID)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .ToListAsync();
            return new ResultPaging<DetailCate>()
            {
                Items = items,
                TotalRecord = total
            };
        }
        public async Task<DetailCate> GetById(int id)
        {
            try
            {
                return await (from a in dbContext.Categories
                              where a.CategoryID == id
                              select new DetailCate()
                              {
                                  LoaiID = a.CategoryID,
                                  TenLoai = a.CategoryName,
                              }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {               
                return null;
            }
        }
    }
}