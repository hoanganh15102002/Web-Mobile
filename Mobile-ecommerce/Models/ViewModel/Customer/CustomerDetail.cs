using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Customer
{
    public class CustomerDetail
    {
        public int MaKH { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string Ten { get; set; }

        public string SoDienThoai { get; set; }

        public string Hinh { get; set; }
        public int GioiTinh { get; set; }
        public string DiaChi { get; set; }     
    }
}