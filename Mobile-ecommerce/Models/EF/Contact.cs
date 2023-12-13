using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class Contact
    {
        [DisplayName("Mã đơn phản hồi")]
        public int ContactID { get; set; }
        [DisplayName("Tên")]
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string tenkh { get; set; }
        [DisplayName("Mail")]
        [Required(ErrorMessage = "Vui lòng nhập mail")]
        public string mail { get; set; }
        [DisplayName("SĐT")]
        [Required(ErrorMessage = "Vui lòng nhập SĐT")]
        public string dienthoai { get; set; }
        [DisplayName("Tiêu đề")]
        [Required(ErrorMessage = "Vui lòng nhập tiêu đề")]
        public string tieude { get; set; }
        [DisplayName("Nội dung")]
        public string noidung { get; set; }
    }
}