using Mobile_ecommerce.Areas.Admin.Model;
using Mobile_ecommerce.Areas.Employee.Model;
using Mobile_ecommerce.Common;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.Common;
using Mobile_ecommerce.Models.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mobile_ecommerce.Models.DAL
{
    public class UserDAL
    {
        private MobileDbContext dbContext;
        public UserDAL()
        {
            dbContext = new MobileDbContext();
        }
        public async Task<bool> UpdateUser(Edit userEdit)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userEdit.Id);
                if (user == null) return false;
                user.UserName = userEdit.Name;
                user.RoleID = userEdit.Quyen;
                user.Status = userEdit.TrangThai;
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<int> Login(string username, string passWord, int quyen)
        {
            try
            {
                var user = await dbContext.Users
                    .Where(x => x.UserName == username && x.RoleID == quyen)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return -1;
                }

                if (user.Status == true)
                {
                    if (user.UserPass == Encryptor.MD5Hash(passWord))
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return 0;
                }

            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public async Task<UserLogin> GetByName_Guest(string name)
        {
            try
            {
                return await (from a in dbContext.Users
                              join b in dbContext.Roles
                              on a.RoleID equals b.RoleID
                              join c in dbContext.Customers
                              on a.UserID equals c.CustomerID
                              where a.UserName == name
                              select new UserLogin()
                              {
                                  UserID = a.UserID,
                                  username = a.UserName,
                                  Role = b.Des,
                                  Address=c.Address,
                                  Phone=c.Phone,
                                  CusName=c.CustomerName
                              }).SingleOrDefaultAsync();
            }
            catch
            {
                return null;
            }
        }

        public async Task<EmpLogin> GetEmpUserName(string username)
        {
            try
            {
                return await dbContext.Users
                    .Where(a => a.UserName == username && a.RoleID == CommonConstants.NHAN_VIEN)
                    .Select(a => new EmpLogin()
                    {
                        Id = a.UserID,
                        Name = a.UserName,
                        Role = "Nhân viên"
                    }).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<AdminLogin> GetAdminUserName(string username)
        {
            try
            {
                return await dbContext.Users
                    .Where(a => a.UserName == username && a.RoleID == CommonConstants.QUAN_TRI)
                    .Select(a => new AdminLogin()
                    {
                        Id = a.UserID,
                        Name = a.UserName,
                        Role = "Quản trị viên"
                    }).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<int> Register_Customer(UserRegister model)
        {
            try
            {
                var check = await dbContext.Users.Where(x => x.UserName.Equals(model.RegisterName.Trim())).FirstOrDefaultAsync();
                if (check != null) return 0;
                var account = new User()
                {
                    UserName = model.RegisterName,
                    UserPass = Encryptor.MD5Hash(model.RegisterPass),
                    RoleID = CommonConstants.KHACH_HANG,
                    Status = true,
                };
                dbContext.Users.Add(account);
                var result = await dbContext.SaveChangesAsync();
                if (result > 0)
                {
                    var member = new Customer()
                    {
                        CustomerID = account.UserID,
                        CustomerName = model.RegisterName,   
                        Email=model.RegisterEmail,
                    };
                    dbContext.Customers.Add(member);
                    return await dbContext.SaveChangesAsync();
                }
                return -1;
            }
            catch (Exception)
            {               
                return -1;
            }
        }
        public async Task<Edit> GetById(int id)
        {
            try
            {
                return await (from a in dbContext.Users
                              join b in dbContext.Roles
                              on a.RoleID equals b.RoleID
                              where a.UserID.Equals(id)
                              select new Edit()
                              {
                                  Id = a.UserID,
                                  Name = a.UserName,                         
                                  Quyen = b.RoleID,
                                  TrangThai = (bool)a.Status
                              }).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<List<Role>> GetAllRole()
        {
            var roles = await dbContext.Roles.ToListAsync();
            return roles;
        }
        public async Task<ResultPaging<UserDetail>> GetList(GetListPaging paging)
        {
            var model = from a in dbContext.Users
                        join b in dbContext.Roles
                        on a.RoleID equals b.RoleID
                        select new UserDetail()
                        {
                            Id = a.UserID,
                            Name = a.UserName,                          
                            Quyen = b.Des,
                            TrangThai = (bool)a.Status
                        };
            if (!string.IsNullOrEmpty(paging.keyWord))
            {
                model = model.Where(x => x.Name.Contains(paging.keyWord.Trim()));
            }

            int total = await model.CountAsync();

            var items = await model.OrderBy(x => x.Id)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .ToListAsync();
            return new ResultPaging<UserDetail>()
            {
                Items = items,
                TotalRecord = total
            };
        }
        public async Task<int> Create(Create user)
        {
            try
            {
                var check = await dbContext.Users.Where(x => x.UserName.Equals(user.Name.Trim())).FirstOrDefaultAsync();
                if (check != null) return 0;
                else
                {
                    var account = new User()
                    {                     
                        UserName = user.Name,
                        UserPass = Encryptor.MD5Hash(user.Password),
                        RoleID = user.Quyen,
                        Status = user.TrangThai
                    };
                    dbContext.Users.Add(account);
                    var result = await dbContext.SaveChangesAsync();
                    return result;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }
        public async Task<bool> Delete(int idTaiKhoan)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(idTaiKhoan);
                if (user == null) return false;
                dbContext.Users.Remove(user);
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> UpdateRole(UserRole userRole)
        {
            try
            {
                var user = await dbContext.Users.FindAsync(userRole.Id);
                if (user == null) return false;
                user.RoleID = userRole.Quyen;
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int NumUser()
        {
            try
            {
                var number = dbContext.Users.Where(x => x.UserID == CommonConstants.QUAN_TRI).Count();
                return number;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int NumEmployee()
        {
            try
            {
                var number = dbContext.Users.Where(x => x.UserID == CommonConstants.NHAN_VIEN).Count();
                return number;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int NumCustomer()
        {
            try
            {
                var number = dbContext.Users.Where(x => x.UserID == CommonConstants.KHACH_HANG).Count();
                return number;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public async Task<UserRole> GetRoleById(int id)
        {
            try
            {
                return await (from a in dbContext.Users
                              join b in dbContext.Roles
                              on a.RoleID equals b.RoleID
                              where a.UserID == id
                              select new UserRole()
                              {
                                  Id = a.UserID,
                                  Name = a.UserName,
                                  Quyen = (int)a.RoleID
                              }).SingleOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}