using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Mobile_ecommerce.Models.DAL;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.Common;
using PagedList;
namespace Mobile_ecommerce.Controllers
{
    public class ProductController : BaseController
    {       
        private MobileDbContext db = new MobileDbContext();   
        // GET: Product
        public ActionResult Index(string searchString,int? page, string sortOrder)
        {
            var products = db.Products.ToList();
            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.ProductName.Contains(searchString)).ToList();
                if (products.Count==0)
                {
                    ViewBag.searchResult = "Không có sản phẩm này vui lòng tìm sản phẩm khác !";
                }
            }          
            switch (sortOrder)
            {
                case "price_asc":
                    products = products.OrderBy(p => p.Price).ToList();
                    break;
                case "price_desc":
                    products = products.OrderByDescending(p => p.Price).ToList();
                    break;
                default:
                    products = products.OrderBy(p => p.ProductName).ToList();
                    break;
            }
            int pageSize = 6;
            int pageNum = (page ?? 1);           
            return View(products.ToPagedList(pageNum, pageSize));
        }    
        public ActionResult SamsungPartial()
        {         
            var pro = db.Products.Where(n => n.Category.CategoryName=="Samsung").Take(4).ToList();
            foreach (var item in pro)
            {
                string values = item.Price.ToString();
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                decimal value = decimal.Parse(values, System.Globalization.NumberStyles.AllowThousands);
                ViewBag.price = string.Format(culture, "{0:N0}", value); ;
            }
            return PartialView(pro);
        }
        public ActionResult IphonePartial()
        {
            var pro = db.Products.Where(n => n.Category.CategoryName == "Iphone").Take(4).ToList();
            foreach (var item in pro)
            {
                string values = item.Price.ToString();
                System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                decimal value = decimal.Parse(values, System.Globalization.NumberStyles.AllowThousands);
                ViewBag.price = string.Format(culture, "{0:N0}", value); ;
            }
            return PartialView(pro);
        }
        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            Session["ProID"] = id;
            var pro = db.Products.Where(n => n.ProductID.Equals(id)).FirstOrDefault();
            string values = pro.Price.ToString();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(values, System.Globalization.NumberStyles.AllowThousands);
            ViewBag.price = string.Format(culture, "{0:N0}", value);
            ViewBag.Cate = pro.CategoryID;
            return View(pro);
        }
        public ActionResult ReviewPartial(int productId)
        {
            var model = new ReviewPro { ProductID = productId };
            return PartialView(model);
        }
        public ActionResult ListReview(int id,int? page)
        {
            Product product = db.Products.Include(p => p.ReviewPros).FirstOrDefault(p => p.ProductID == id);
            int pageSize = 4;
            int pageNum = (page ?? 1);
            return PartialView(product.ReviewPros.ToPagedList(pageNum, pageSize));
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddReview(ReviewPro review)
        {
            int id = int.Parse(Session["ProID"].ToString());
            var pro = db.Products.Where(n => n.ProductID.Equals(id)).FirstOrDefault();
            string values = pro.Price.ToString();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(values, System.Globalization.NumberStyles.AllowThousands);
            ViewBag.price = string.Format(culture, "{0:N0}", value);
            ViewBag.Cate = pro.CategoryID;
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                db.ReviewPros.Add(review);
                db.SaveChanges();
                TempData["AlertMessage"] = "Nhận xét thành công !";
                TempData["AlertType"] = "alert-success";
                return RedirectToAction("Details", new { id = review.ProductID });
            }
            ViewBag.error = "Vui lòng nhập thông tin nhận xét !";
            return View("Details",pro);
        }
    }
}
