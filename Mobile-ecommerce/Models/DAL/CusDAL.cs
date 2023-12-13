using Mobile_ecommerce.Areas.Admin.Model;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.Common;
using Mobile_ecommerce.Models.ViewModel.Contact;
using Mobile_ecommerce.Models.ViewModel.Customer;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mobile_ecommerce.Models.DAL
{
    public class CusDAL
    {
        private MobileDbContext dbContext;
        public CusDAL()
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
        public async Task<Detail> GetByIdContact(int id)
        {
            try
            {
                return await (from a in dbContext.Contacts
                              where a.ContactID == id
                              select new Detail()
                              {
                                  ContactID=a.ContactID,
                                  dienthoai=a.dienthoai,
                                  noidung=a.noidung,
                                  tieude=a.tieude,
                                  mail=a.mail,
                                  tenkh=a.tenkh
                              }).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<Edit> GetById(int id)
        {
            try
            {
                return await (from a in dbContext.Customers                                                        
                              where a.CustomerID == id
                              select new Edit()
                              {
                                  MaKH = a.CustomerID,
                                  Name = a.CustomerName,
                                  Phone = a.Phone,
                                  Mail=a.Email,
                                  Image = a.Image,
                                  Gender = a.Gender,
                                  Address = a.Address,                                                  
                              }).SingleOrDefaultAsync();              
            }
            catch (Exception)
            {
                return null;
            }
        }
        public CustomerEditClient GetCustomer(int id)
        {
            try
            {
                var item = dbContext.Customers.Where(x => x.CustomerID.Equals(id)).SingleOrDefault();
                if (item == null) return null;
                return new CustomerEditClient()
                {
                    MaKH = item.CustomerID,
                    AnhDaiDien = item.Image,
                    Ten = item.CustomerName,
                    GioiTinh = item.Gender,
                    DiaChi = item.Address,
                    SoDienThoai = item.Phone,                             
                };
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<ResultPaging<Detail>> GetListContact(GetListPaging paging)
        {
            IQueryable<Contact> model = dbContext.Contacts;
            if (!string.IsNullOrEmpty(paging.keyWord))
            {
                model = model.Where(x => x.tenkh.Contains(paging.keyWord.Trim()) || x.mail.Contains(paging.keyWord.Trim()));
            }

            int total = await model.CountAsync();

            var items = await model.OrderBy(x => x.ContactID)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .Select(item => new Detail()
                {              
                    ContactID=item.ContactID,
                    tenkh = item.tenkh,
                    mail = item.mail,
                    tieude = item.tieude,
                    noidung = item.noidung,
                    dienthoai = item.dienthoai,
                }).ToListAsync();
            return new ResultPaging<Detail>()
            {
                Items = items,
                TotalRecord = total
            };
        }
        public async Task<ResultPaging<CustomerDetail>> GetList(GetListPaging paging)
        {
            IQueryable<Customer> model = dbContext.Customers;
            if (!string.IsNullOrEmpty(paging.keyWord))
            {
                model = model.Where(x => x.CustomerName.Contains(paging.keyWord.Trim()) || x.Phone.Contains(paging.keyWord.Trim()));
            }

            int total = await model.CountAsync();

            var items = await model.OrderBy(x => x.CustomerID)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .Select(item => new CustomerDetail()
                {
                    MaKH = item.CustomerID,
                    Ten = item.CustomerName,
                    SoDienThoai = item.Phone,
                    Hinh = item.Image,
                    GioiTinh = item.Gender,
                    DiaChi = item.Address,                                 
                }).ToListAsync();
            return new ResultPaging<CustomerDetail>()
            {
                Items = items,
                TotalRecord = total
            };
        }
        //Admin
        public async Task<bool> EditCus(Edit cusEdit, int id, HttpServerUtilityBase httpServer)
        {
            try
            {
                var cus = await dbContext.Customers.FindAsync(id);
                if (cus == null) return false;
                if (cusEdit.ImageUpload != null && cusEdit.ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(cusEdit.ImageUpload.FileName);
                    string extension = Path.GetExtension(cusEdit.ImageUpload.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    cusEdit.Image = "/Asset/Admin/img/" + fileName.Trim();
                    fileName = Path.Combine(httpServer.MapPath("/Asset/Admin/img/"), fileName);
                    cusEdit.ImageUpload.SaveAs(fileName);
                }
                cus.CustomerName = cusEdit.Name;
                cus.Address = cusEdit.Address;
                cus.Email = cusEdit.Mail;
                cus.Phone = cusEdit.Phone;
                cus.Gender = cusEdit.Gender;
                cus.Image = cusEdit.Image;
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<int> Create(Create customer, HttpServerUtilityBase httpServer)
        {
            try
            {
                var check = await dbContext.Customers.Include(n => n.User).Where(x => x.CustomerID.Equals(customer.MaKH)).SingleOrDefaultAsync();
                if (check != null) return 0;
                else
                {
                    if (customer.ImageUpload != null && customer.ImageUpload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(customer.ImageUpload.FileName);
                        string extension = Path.GetExtension(customer.ImageUpload.FileName);
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        customer.Image = "/Asset/Client/img/info/" + fileName.Trim();
                        fileName = Path.Combine(httpServer.MapPath("/Asset/Client/img/info/"), fileName);
                        customer.ImageUpload.SaveAs(fileName);
                    }
                    var member = new Customer()
                    {
                        CustomerID = customer.MaKH,
                        CustomerName = customer.Name,
                        Gender = customer.Gender,
                        Email = customer.Mail,
                        Image = customer.Image,
                        Address = customer.Address,
                        Phone = customer.Phone,
                    };
                    dbContext.Customers.Add(member);
                    var result = await dbContext.SaveChangesAsync();
                    return result;
                }           
            }
            catch (Exception)
            {
                return -1;
            }
        }
        public async Task<bool> Delete(int id)
        {
            try
            {
                var member = await dbContext.Customers.FindAsync(id);
                if (member == null) return false;
                dbContext.Customers.Remove(member);
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
      
        //Client
        public async Task<CustomerEditClient> GetByIdClient(int id)
        {
            try
            {
                var item = await dbContext.Customers.Where(x => x.CustomerID.Equals(id)).SingleOrDefaultAsync();
                if (item == null) return null;
                return new CustomerEditClient()
                {
                    MaKH = item.CustomerID,
                    AnhDaiDien = item.Image,
                    Ten = item.CustomerName,
                    GioiTinh =item.Gender,
                    Mail=item.Email,
                    DiaChi = item.Address,
                    SoDienThoai = item.Phone,
                };
            }
            catch (Exception)
            {
                return null;
            }
        }       
        //Client
        public async Task<bool> UpdateClient(CustomerEditClient item, HttpServerUtilityBase httpServer)
        {
            try
            {
                var member = await dbContext.Customers.Where(x => x.CustomerID.Equals(item.MaKH)).SingleOrDefaultAsync();
                if (member == null) return false;

                if (item.ImageMain != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(item.ImageMain.FileName);
                    string extension = Path.GetExtension(item.ImageMain.FileName);
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    item.AnhDaiDien = "/Asset/Client/img/info/" + fileName;
                    fileName = Path.Combine(httpServer.MapPath("/Asset/Client/img/info/"), fileName);
                    item.ImageMain.SaveAs(fileName);
                }
                member.Image = item.AnhDaiDien;
                member.CustomerName = item.Ten;
                member.Gender = item.GioiTinh;
                member.Address = item.DiaChi;
                member.Email = item.Mail;
                member.Phone = item.SoDienThoai;             
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}