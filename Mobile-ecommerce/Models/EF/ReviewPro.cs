using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class ReviewPro
    {
        [Key]
        public int ReviewProID { get; set; }
        [Display(Name ="Đánh giá")]
        public int RateValue { get; set; }
        [Display(Name = "Tên khách hàng")]
        public string ReviewerName { get; set; }
        [Display(Name = "Nhận xét")]
        public string ReviewContent { get; set; }
        [Display(Name = "Ngày đăng")]
        public DateTime ReviewDate { get; set; }

        public int ProductID { get; set; }
        public Product Product { get; set; }
    }
}