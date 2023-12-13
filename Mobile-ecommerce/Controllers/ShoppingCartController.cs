using DemoVNPay.Others;
using Mobile_ecommerce.Common;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.User;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Mobile_ecommerce.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private MobileDbContext db;
        public ShoppingCartController()
        {
            db = new MobileDbContext();
        }
        public Cart GetCart()
        {
            Cart cart = Session["cart"] as Cart;
            if (cart == null || Session["cart"] == null)
            {
                cart = new Cart();
                Session["cart"] = cart;
            }
            return cart;
        }
        // GET: ShoppingCart
        public ActionResult Index()
        {
            if (Session["cart"] == null)
            {
                return View();
            }
            Cart giohang = Session["cart"] as Cart;
            string a = giohang.GetTotal().ToString();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(a, System.Globalization.NumberStyles.AllowThousands);
            ViewBag.price = string.Format(culture, "{0:N0}", value);
            return View(giohang);
        }

        public ActionResult AddToCart(int id)
        {         
            var product = db.Products.SingleOrDefault(p => p.ProductID == id);
            if (product != null)
            {           
                GetCart().AddItem(product);
            }
            TempData["AlertMessage"] = "Thêm vào giỏ hàng thành công !";
            TempData["AlertType"] = "alert-success";
            return RedirectToAction("Index","ShoppingCart");
        }
        public ActionResult RemoveFromCart(int productId)
        {
            Cart cart = Session["cart"] as Cart;
            Product product = db.Products
                .SingleOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.RemoveItem(productId);
            }
            TempData["AlertMessage"] = "Xóa thành công !";
            TempData["AlertType"] = "alert-danger";
            return RedirectToAction("Index", "ShoppingCart");
        }
        public ActionResult UpdateFromCart(FormCollection form)
        {
            Cart cart = Session["Cart"] as Cart;
            int idPro = int.Parse(form["IDPro"]);
            int soluong = int.Parse(form["SL"]);
            cart.UpdateItem(idPro, soluong);
            TempData["AlertMessage"] = "Cập nhật thành công !";
            TempData["AlertType"] = "alert-success";
            return RedirectToAction("Index", "ShoppingCart");
        }
        public PartialViewResult BagCart()
        {
            int totalitem = 0;
            Cart cart = Session["cart"] as Cart;
            if (cart != null)
            {
                totalitem = cart.TotalQuantity();
            }
            ViewBag.Listitem = totalitem;
            return PartialView("BagCart");
        }

        public ActionResult Order()
        {
            if (Session[CommonConstants.USER_SESSION] == null)
            {
                return RedirectToAction("Index", "UserClient");
            }
            Cart cart = GetCart();
            if (cart == null)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }
            var id = (UserLogin)(Session[CommonConstants.USER_SESSION]);
            var getid = db.Customers.Where(n => n.CustomerID.Equals(id.UserID)).FirstOrDefault();
            ViewBag.Info = getid;
            ViewBag.Totalquantity = cart.TotalQuantity();
            string a = cart.GetTotal().ToString();
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
            decimal value = decimal.Parse(a, System.Globalization.NumberStyles.AllowThousands);
            ViewBag.price = string.Format(culture, "{0:N0}", value); 
            return View(cart);
        }      
        public ActionResult SuccessPage()
        {
            return View();
        }
        public ActionResult GetVoucher(FormCollection form)
        {        
            string mavoucher = (form["mavoucher"]).Trim().ToString();
            var id = (UserLogin)(Session[CommonConstants.USER_SESSION]);
            var getid = db.Customers.Where(n => n.CustomerID.Equals(id.UserID)).FirstOrDefault();
            ViewBag.Info = getid;
            Cart cart = GetCart();
            if (!string.IsNullOrEmpty(mavoucher))
            {
                var check = db.Vouchers.Where(n => n.VoucherID.Contains(mavoucher)).FirstOrDefault();
                if (check == null)
                {
                    ViewBag.messageOn = "Mã không hợp lệ hoặc không tồn tại";
                    return View("Order", cart);
                }
                else
                {
                    decimal apdungvoucher = cart.GetTotal() * check.Discount;
                    string formatDecimal = apdungvoucher.ToString("0.#");
                    decimal totalMoney = cart.GetTotal() - decimal.Parse(formatDecimal);
                    System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-US");
                    string a = totalMoney.ToString();
                    decimal value = decimal.Parse(a, System.Globalization.NumberStyles.AllowThousands);
                    Session["TotalWithVoucher"] = string.Format(culture, "{0:N0}", value);
                    ViewBag.messageOn = "Áp dụng thành công";
                    Session["CouponCode"] = mavoucher;                   
                    return View("Order", cart);
                }
            }
            ViewBag.messageOn = "Mã không hợp lệ hoặc không tồn tại";
            return View("Order", cart);
        }
        public ActionResult SuccessOrder()
        {
            Cart cart = GetCart();
            Order order = new Order();
            var id = (UserLogin)(Session[CommonConstants.USER_SESSION]);
            var getid = db.Customers.Where(n => n.CustomerID.Equals(id.UserID)).FirstOrDefault();
            ViewBag.Info = getid;     
            order.CustomerID = id.UserID;
            order.OrderDate = DateTime.Now;
            if (Session["TotalWithVoucher"] == null)
            {
                order.Total = cart.GetTotal();
            }
            else
            {
                order.Total = decimal.Parse(Session["TotalWithVoucher"].ToString());
            }
            if (Session["CouponCode"] != null)
            {
                order.VoucherID = Session["CouponCode"].ToString();
            }
            order.Status = 1;
            order.ShippingStatus = 1;
            db.Orders.Add(order);
            db.SaveChanges();
            foreach(var item in cart.Items)
            {
                OrderDetail details = new OrderDetail();
                details.OrderID = order.OrderID;
                details.ProductID = item.Product.ProductID;
                details.Quantity = item.Quantity;
                details.TotalMoney = item.TotalPrice;
                db.OrderDetails.Add(details);           
            }
            db.SaveChanges();
            cart.Clear();
            return RedirectToAction("SuccessPage");
        }
        public ActionResult Payment()
        {
            decimal totalMoney = 0;
            Cart cart = GetCart();
            if (Session["CouponCode"] != null)
            {
                string mavoucher = Session["CouponCode"].ToString();
                if (!string.IsNullOrEmpty(mavoucher))
                {
                    var check = db.Vouchers.Where(n => n.VoucherID.Contains(mavoucher)).FirstOrDefault();
                    decimal apdungvoucher = cart.GetTotal() * check.Discount;
                    string formatDecimal = apdungvoucher.ToString("0.#");
                    totalMoney = cart.GetTotal() - decimal.Parse(formatDecimal);
                }
            }                            
            string url = ConfigurationManager.AppSettings["Url"];
            string returnUrl = ConfigurationManager.AppSettings["ReturnUrl"];
            string tmnCode = ConfigurationManager.AppSettings["TmnCode"];
            string hashSecret = ConfigurationManager.AppSettings["HashSecret"];

            PayLib pay = new PayLib();              
            if (totalMoney != 0)
            {
                pay.AddRequestData("vnp_Amount", totalMoney.ToString());
            }
            else
            {  
                pay.AddRequestData("vnp_Amount",cart.GetTotal().ToString()); //số tiền cần thanh toán, công thức: số tiền * 100 - ví dụ 10.000 (mười nghìn đồng) --> 1000000
            }
            pay.AddRequestData("vnp_Version", "2.1.0"); //Phiên bản api mà merchant kết nối. Phiên bản hiện tại là 2.1.0
            pay.AddRequestData("vnp_Command", "pay"); //Mã API sử dụng, mã cho giao dịch thanh toán là 'pay'
            pay.AddRequestData("vnp_TmnCode", tmnCode); //Mã website của merchant trên hệ thống của VNPAY (khi đăng ký tài khoản sẽ có trong mail VNPAY gửi về)
            pay.AddRequestData("vnp_BankCode", ""); //Mã Ngân hàng thanh toán (tham khảo: https://sandbox.vnpayment.vn/apis/danh-sach-ngan-hang/), có thể để trống, người dùng có thể chọn trên cổng thanh toán VNPAY
            pay.AddRequestData("vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss")); //ngày thanh toán theo định dạng yyyyMMddHHmmss
            pay.AddRequestData("vnp_CurrCode", "VND"); //Đơn vị tiền tệ sử dụng thanh toán. Hiện tại chỉ hỗ trợ VND
            pay.AddRequestData("vnp_IpAddr", Util.GetIpAddress()); //Địa chỉ IP của khách hàng thực hiện giao dịch
            pay.AddRequestData("vnp_Locale", "vn"); //Ngôn ngữ giao diện hiển thị - Tiếng Việt (vn), Tiếng Anh (en)
            pay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang"); //Thông tin mô tả nội dung thanh toán
            pay.AddRequestData("vnp_OrderType", "other"); //topup: Nạp tiền điện thoại - billpayment: Thanh toán hóa đơn - fashion: Thời trang - other: Thanh toán trực tuyến
            pay.AddRequestData("vnp_ReturnUrl", returnUrl); //URL thông báo kết quả giao dịch khi Khách hàng kết thúc thanh toán
            pay.AddRequestData("vnp_TxnRef", DateTime.Now.Ticks.ToString()); //mã hóa đơn

            string paymentUrl = pay.CreateRequestUrl(url, hashSecret);

            return Redirect(paymentUrl);
        }

        public ActionResult PaymentConfirm()
        {
            if (Request.QueryString.Count > 0)
            {
                string hashSecret = ConfigurationManager.AppSettings["HashSecret"]; //Chuỗi bí mật
                var vnpayData = Request.QueryString;
                PayLib pay = new PayLib();

                //lấy toàn bộ dữ liệu được trả về
                foreach (string s in vnpayData)
                {
                    if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                    {
                        pay.AddResponseData(s, vnpayData[s]);
                    }
                }

                long orderId = Convert.ToInt64(pay.GetResponseData("vnp_TxnRef")); //mã hóa đơn
                long vnpayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo")); //mã giao dịch tại hệ thống VNPAY
                string vnp_ResponseCode = pay.GetResponseData("vnp_ResponseCode"); //response code: 00 - thành công, khác 00 - xem thêm https://sandbox.vnpayment.vn/apis/docs/bang-ma-loi/
                string vnp_SecureHash = Request.QueryString["vnp_SecureHash"]; //hash của dữ liệu trả về

                bool checkSignature = pay.ValidateSignature(vnp_SecureHash, hashSecret); //check chữ ký đúng hay không?

                if (checkSignature)
                {
                    if (vnp_ResponseCode == "00")
                    {
                        //Thanh toán thành công
                        ViewBag.Message = "Thanh toán thành công hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId;
                        Cart cart = GetCart();
                        Order order = new Order();
                        var id = (UserLogin)(Session[CommonConstants.USER_SESSION]);
                        order.CustomerID = id.UserID;
                        order.OrderDate = DateTime.Now;
                        if (Session["TotalWithVoucher"] == null)
                        {
                            order.Total = cart.GetTotal();
                        }
                        else
                        {
                            order.Total = decimal.Parse(Session["TotalWithVoucher"].ToString());
                        }
                        if (Session["CouponCode"] != null)
                        {
                            order.VoucherID = Session["CouponCode"].ToString();
                        }
                        db.Orders.Add(order);
                        db.SaveChanges();
                        foreach (var item in cart.Items)
                        {
                            OrderDetail details = new OrderDetail();
                            details.OrderID = order.OrderID;
                            details.ProductID = item.Product.ProductID;
                            details.Quantity = item.Quantity;
                            details.TotalMoney = item.TotalPrice;
                            db.OrderDetails.Add(details);
                        }
                        db.SaveChanges();
                        cart.Clear();
                    }
                    else
                    {
                        //Thanh toán không thành công. Mã lỗi: vnp_ResponseCode
                        ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý hóa đơn " + orderId + " | Mã giao dịch: " + vnpayTranId + " | Mã lỗi: " + vnp_ResponseCode;
                    }
                }
                else
                {
                    ViewBag.Message = "Có lỗi xảy ra trong quá trình xử lý";
                }
            }

            return View();
        }
    }
}