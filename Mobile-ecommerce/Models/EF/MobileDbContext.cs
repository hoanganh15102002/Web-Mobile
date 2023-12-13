using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Threading.Tasks;
using Mobile_ecommerce.Models.ViewModel.Common;

namespace Mobile_ecommerce.Models.EF
{
    public class MobileDbContext:DbContext
    {
        public MobileDbContext() : base("name=MobileDbContext")
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Customer> Customers { get; set; }               
        public DbSet<Product> Products { get; set; }       
        public DbSet<Category> Categories { get; set; }   
        public DbSet<Order> Orders { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<ReviewPro> ReviewPros { get; set; }

        internal Task GetList(GetListPaging request)
        {
            throw new NotImplementedException();
        }
    }
}