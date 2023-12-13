using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Contact
{
    public class Detail
    {
        [DisplayName("Mã đơn phản hồi")]
        public int ContactID { get; set; }
        [DisplayName("Tên")]   
        public string tenkh { get; set; }
        [DisplayName("Mail")]  
        public string mail { get; set; }
        [DisplayName("SĐT")]   
        public string dienthoai { get; set; }
        [DisplayName("Tiêu đề")]
        public string tieude { get; set; }
        [DisplayName("Nội dung")]
        public string noidung { get; set; }
    }
}