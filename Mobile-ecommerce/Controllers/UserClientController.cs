using Mobile_ecommerce.Common;
using Mobile_ecommerce.Models.DAL;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Mobile_ecommerce.Controllers
{
    public class UserClientController : BaseController
    {
        MobileDbContext db = new MobileDbContext();
        private readonly UserDAL userDAL;
        private readonly CusDAL cusDAL;
        public UserClientController()
        {
            userDAL = new UserDAL();
            cusDAL = new CusDAL();
        }
        [HttpGet]
        public ActionResult Index()
        {
            Session[CommonConstants.USER_SESSION] = null;
            return View();
        }          
        [HttpPost]
        public async Task<ActionResult> Index(LoginClient model)
        {
            if (ModelState.IsValid)
            {
                if (model.login_password == null)
                {
                    ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng !";
                    return View("Index");
                }
                string hashPass = Encryptor.MD5Hash(model.login_password);
                var check = db.Users.Where(m => m.UserName.Equals(model.login_name) && m.UserPass.Equals(hashPass)).FirstOrDefault();
                if (check == null)
                {
                    ViewBag.error = "Tên đăng nhập hoặc mật khẩu không đúng !";
                    return View("Index");
                }
                else
                {
                    var userLogin = await userDAL.GetByName_Guest(model.login_name);
                    Session[CommonConstants.USER_SESSION] = userLogin;
                    SetAlert("Đăng nhập thành công !", "success");
                    return RedirectToAction("Index","Home");
                }
            }
            return View();
        }
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Register(UserRegister model)
        {
            if (ModelState.IsValid)
            {
                if (model.RegisterPass == model.ConfirmPass)
                {
                    var result = await userDAL.Register_Customer(model);
                    if (result > 0)
                    {
                        SetAlert("Đăng ký thành công !", "success");
                        return View("Index");
                    }
                    else if (result == 0)
                    {
                        SetAlert("Đã có lỗi xảy ra. Vui lòng thử lại", "error");
                        ViewBag.error = "Thông tin tài khoản bị sai !";
                        return View();
                    }                 
                }             
            }
            ViewBag.error = "Vui lòng nhập đầy đủ thông tin !";
            return View();
        }        
    }
}