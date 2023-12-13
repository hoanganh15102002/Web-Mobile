using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Customer
{
    public class CustomerEditClient
    {
        public int MaKH { get; set; }

        public string Ten { get; set; }

        public string SoDienThoai { get; set; }

        public string Mail { get; set; }
        public int GioiTinh { get; set; }
        public string DiaChi { get; set; }
       
        public string AnhDaiDien { get; set; }
      
        public HttpPostedFileBase ImageMain { get; set; }

    }
}