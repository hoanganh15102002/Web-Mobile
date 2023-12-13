using Mobile_ecommerce.Areas.Admin.Model;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.Common;
using Mobile_ecommerce.Models.ViewModel.Voucher;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Mobile_ecommerce.Models.DAL
{
    public class VoucherDAL
    {
        private MobileDbContext dbContext;
        public VoucherDAL()
        {
            dbContext = new MobileDbContext();
        }
        public async Task<int> Create(Edit pro)
        {
            try
            {
                var check = await dbContext.Vouchers.Where(x => x.VoucherID.Equals(pro.VoucherID)).FirstOrDefaultAsync();
                if (check != null) return 0;
                else
                {
                    var type = new Voucher()
                    {
                        VoucherID = pro.VoucherID,
                        Discount = pro.Value,
                        StartDate = DateTime.Parse(pro.DateStart),
                        EndDate = DateTime.Parse(pro.DateEnd),
                        VouDes = pro.Des,
                        VoucherName = pro.Name
                    };
                    dbContext.Vouchers.Add(type);
                    var result = await dbContext.SaveChangesAsync();
                    return result;
                }
            }
            catch
            {
                return -1;
            }
        }
        public async Task<bool> Update(Edit edit)
        {
            try
            {
                var pro = await dbContext.Vouchers.FindAsync(edit.VoucherID);
                pro.VoucherID = edit.VoucherID;
                pro.VoucherName = edit.Name;
                pro.VouDes = edit.Des;
                pro.StartDate = DateTime.Parse(edit.DateStart);
                pro.EndDate = DateTime.Parse(edit.DateEnd);
                pro.Discount = edit.Value;
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<bool> Delete(string id)
        {
            try
            {
                var pro = await dbContext.Vouchers.FindAsync(id.Trim());
                if (pro == null) return false;
                dbContext.Vouchers.Remove(pro);
                return await dbContext.SaveChangesAsync() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public async Task<ResultPaging<DetailVoucher>> GetList(GetListPaging paging)
        {
            var model = from a in dbContext.Vouchers
                        select new DetailVoucher()
                        {
                            VoucherID = a.VoucherID,
                            DateStart = a.StartDate.ToString(),
                            DateEnd = a.EndDate.ToString(),
                            Name = a.VoucherName,
                            Value = a.Discount
                        };
            if (!string.IsNullOrEmpty(paging.keyWord))
            {
                model = model.Where(x => x.VoucherID.Contains(paging.keyWord.Trim()));
            }

            int total = await model.CountAsync();

            var items = await model.OrderBy(x => x.VoucherID)
                .Skip((paging.PageIndex - 1) * paging.PageSize).Take(paging.PageSize)
                .ToListAsync();
            return new ResultPaging<DetailVoucher>()
            {
                Items = items,
                TotalRecord = total
            };
        }
        public async Task<Edit> GetById(string id)
        {
            try
            {
                return await (from a in dbContext.Vouchers
                              where a.VoucherID.Contains(id.Trim())
                              select new Edit()
                              {
                                  VoucherID = a.VoucherID,
                                  Name = a.VoucherName,         
                                  Des=a.VouDes,
                                  DateStart = a.StartDate.ToString(),
                                  DateEnd = a.EndDate.ToString(),
                                  Value = a.Discount
                              }).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}