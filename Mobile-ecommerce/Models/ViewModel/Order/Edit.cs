using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Order
{
    public class Edit
    {
        [Display(Name = "Mã hóa đơn")]
        public int OrderID { get; set; }
        [Display(Name = "Ngày tạo hóa đơn")]
        public string Ngay { get; set; }
        [Display(Name = "Tổng tiền")]
        public decimal TongTien { get; set; }
        [Display(Name = "Tình trạng")]
        public int Status { get; set; }
        [Display(Name = "Mã khách hàng")]
        public int CustomerID { get; set; }

        [Display(Name = "Tên khách hàng")]
        public string Name { get; set; }

        [Display(Name = "Tình trạng giao hàng")]
        public int Shipping { get; set; }
    }
}