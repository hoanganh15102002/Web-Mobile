using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Order
{
    public class OrderInfo
    {
        [Display(Name = "Số lượng đặt")]
        public int Quantity { get; set; }
        [Display(Name = "Tổng tiền")]
        public decimal Price { get; set; }
        [Display(Name = "Sản phẩm")]
        public string Name { get; set; }
    }
}