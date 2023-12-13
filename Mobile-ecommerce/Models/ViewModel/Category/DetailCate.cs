using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Mobile_ecommerce.Models.ViewModel.Category
{
    public class DetailCate
    {
        [Display(Name = "STT")]
        public int LoaiID { get; set; }
        [Display(Name = "Loại sản phẩm")]
        public string TenLoai { get; set; }
    }
}