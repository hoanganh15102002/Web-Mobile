using Mobile_ecommerce.Areas.Admin.Model;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.Common;
using Mobile_ecommerce.Models.ViewModel.Product;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace Mobile_ecommerce.Models.DAL
{
    public class ProductDAL
    {
        private MobileDbContext dbContext;
        public ProductDAL()
        {
            dbContext = new MobileDbContext();
        }
        public Create Details()
        {
            try
            {
                return new Create();
            }
            catch
            {
                return null;
            }
        }
        public async Task<int> Create(Create pro, HttpServerUtilityBase httpServer)
        {
            try
            {
                var check = await dbContext.Products.Where(x => x.ProductID.Equals(pro.ProductID)).FirstOrDefaultAsync();
                if (check != null) return 0;
                else
                {
                    if (pro.ImageUpload != null&& pro.ImageUpload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(pro.ImageUpload.FileName);
                        string extension = Path.GetExtension(pro.ImageUpload.FileName);                       
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.Image = "/Asset/Admin/img/" + fileName.Trim();                     
                        fileName = Path.Combine(httpServer.MapPath("/Asset/Admin/img/"), fileName);                      
                        pro.ImageUpload.SaveAs(fileName);                     
                    }
                    if(pro.ImageProItem != null && pro.ImageProItem.ContentLength>0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(pro.ImageProItem.FileName);
                        string extension = Path.GetExtension(pro.ImageProItem.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        pro.ImagePro = "/Asset/Admin/img/" + fileName.Trim();
                        fileName = Path.Combine(httpServer.MapPath("/Asset/Admin/img/"), fileName);
                        pro.ImageProItem.SaveAs(fileName);
                    }
                    var product = new Product()
                    {
                        ProductID = pro.ProductID,
                        ProductName = pro.ProductName,
                        CategoryID = pro.Category,
                        ProDes = pro.Description,
                        Price = pro.Price,
                        Images = pro.Image,
                        ImageNote = pro.ImagePro,                    
                        Quantity = pro.Quantity,            
                        Color=pro.Color,
                        InfoDes=pro.InfoPro
                    };
                    dbContext.Products.Add(product);
                    var result = await dbContext.SaveChangesAsync();
                    return result;
                }
            }
            catch
            {
                return -1;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var pro = await dbContext.Products.FindAsync(id);
                if (pro == null) return false;
                dbContext.Products.Remove(pro);
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdatePro(Edit edit, int id, HttpServerUtilityBase httpServer)
        {
            try
            {
                var pro = await dbContext.Products.FindAsync(id);
                if (pro == null) return false;
                if (edit.ImageUpload != null && edit.ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(edit.ImageUpload.FileName);
                    string extension = Path.GetExtension(edit.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    edit.Image = "/Asset/Admin/img/" + fileName.Trim();
                    fileName = Path.Combine(httpServer.MapPath("/Asset/Admin/img/"), fileName);
                    edit.ImageUpload.SaveAs(fileName);
                }
                if (edit.ImageProItem != null && edit.ImageProItem.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(edit.ImageProItem.FileName);
                    string extension = Path.GetExtension(edit.ImageProItem.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    edit.ImagePro = "/Asset/Admin/img/" + fileName.Trim();
                    fileName = Path.Combine(httpServer.MapPath("/Asset/Admin/img/"), fileName);
                    edit.ImageProItem.SaveAs(fileName);
                }
                pro.ProductName = edit.ProductName;
                pro.CategoryID = edit.Category;
                pro.ProDes = edit.Description;
                pro.InfoDes = edit.InfoPro;
                pro.Price = edit.Price;           
                pro.Quantity = edit.Quantity;
                pro.Images = edit.Image;
                pro.ImageNote = edit.ImagePro;
                pro.Color = edit.Color;
                pro.InfoDes = edit.InfoPro;
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        
        public async Task<ResultPaging<ProductDetail>> GetList(GetListPaging paging)
        {
            var model = from a in dbContext.Products
                        join b in dbContext.Categories
                        on a.CategoryID equals b.CategoryID                                       
                        select new ProductDetail()
                        {
                            ProductID = a.ProductID,
                            ProductName = a.ProductName,
                            Category = b.CategoryName,
                            Price = a.Price.ToString(),
                            Quantity = a.Quantity,
                            Image = a.Images,
                            Color=a.Color,                  
                        };
            if (!string.IsNullOrEmpty(paging.keyWord))
            {
                model = model.Where(x => x.ProductName.Contains(paging.keyWord.Trim()));
            }

            int total = await model.CountAsync();

            var items = await model.OrderBy(x => x.ProductID)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .ToListAsync();
            return new ResultPaging<ProductDetail>()
            {
                Items = items,
                TotalRecord = total
            };
        }
        public async Task<List<Category>> GetAllCategories()
        {
            var cate = await dbContext.Categories.ToListAsync();
            return cate;
        }
       
        public async Task<Edit> GetById(int id)
        {
            try
            {
                return await (from a in dbContext.Products
                              join b in dbContext.Categories
                              on a.CategoryID equals b.CategoryID                           
                              where a.ProductID.Equals(id)
                              select new Edit()
                              {
                                  ProductName = a.ProductName,
                                  Category = b.CategoryID,
                                  Description = a.ProDes,
                                  Price = a.Price,
                                  Image = a.Images,
                                  ImagePro = a.ImageNote,
                                  InfoPro=a.InfoDes,
                                  Quantity = a.Quantity,                                   
                                  Color=a.Color,
                              }).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}