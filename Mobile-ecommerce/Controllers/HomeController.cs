using Mobile_ecommerce.Common;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mobile_ecommerce.Models.ViewModel.User;
using Mobile_ecommerce.Models.ViewModel.Customer;

namespace Mobile_ecommerce.Controllers
{
    public class HomeController : BaseController
    {
        private CusDAL cus;
        private MobileDbContext dbContext = new MobileDbContext();
        public HomeController()
        {
            cus = new CusDAL();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contact(Contact form)
        {
            if (ModelState.IsValid)
            {
                dbContext.Contacts.Add(form);
                dbContext.SaveChanges();
                SetAlert("Gửi đơn thành công", "success");
                return View();
            }
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            SetAlert("Đăng xuất thành công !", "success");
            return Redirect("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Info()
        {
            var session = (UserLogin)Session[CommonConstants.USER_SESSION];
            if (session == null) return RedirectToAction("Index");
            var member = await cus.GetByIdClient(session.UserID);
            ViewBag.AnhDaiDien = member.AnhDaiDien;
            return View(member);
        }
        [HttpPost]
        public async Task<ActionResult> Info(CustomerEditClient member)
        {          
            var result = await cus.UpdateClient(member, Server);
            if (result)
            {
                SetAlert("Cập nhật thành công", "success");
                return RedirectToAction("Info");
            }
            return View();
        }
    }
}