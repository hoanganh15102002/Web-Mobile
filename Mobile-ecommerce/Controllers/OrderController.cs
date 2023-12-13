using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Mobile_ecommerce.Common;
using Mobile_ecommerce.Models.DAL;
using Mobile_ecommerce.Models.EF;
using Mobile_ecommerce.Models.ViewModel.Common;
using Mobile_ecommerce.Models.ViewModel.Order;
using Mobile_ecommerce.Models.ViewModel.User;

namespace Mobile_ecommerce.Controllers
{
    public class OrderController : BaseController
    {
        OrderDAL db = new OrderDAL();
        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetPaging(string keyWord, int pageIndex = 1, int pageSize = 5)
        {
            var request = new GetListPaging()
            {
                keyWord = keyWord,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
            var id = (UserLogin)(Session[CommonConstants.USER_SESSION]);
            var data = await db.GetList(request,id.UserID);
            int totalRecord = data.TotalRecord;
            int totalPage = (int)Math.Ceiling((double)totalRecord / pageSize);
            return Json(new { data = data.Items, pageCurrent = pageIndex, totalPage = totalPage, totalRecord = totalRecord }
                , JsonRequestBehavior.AllowGet);

            // Datetime:  .NET JavaScriptSerializer
        }
        // GET: Order/Details/5
        public async Task<ActionResult> Details(int id)
        {
           var pro = await db.GetByIdInfo(id);
            pro.ToList();
            return View(pro);
        }
        [HttpPost]
        public async Task<ActionResult> Edit(int id)
        {
            var result = await db.Edit(id);
            if (result)
            {
                SetAlert("Hủy thành công", "success");
            }
            else
            {
                SetAlert("Có lỗi xảy ra. Vui lòng thử lại!", "error");
            }
            return Json(result);
        }
    }
}
