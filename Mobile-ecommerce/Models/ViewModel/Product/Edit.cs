using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Mvc;

namespace Mobile_ecommerce.Models.ViewModel.Product
{
    public class Edit
    {
        public Edit()
        {
            Image = "~/Asset/Admin/img/add.jpg";
            ImagePro = "~/Asset/Admin/img/addnote.jpg";
        }
        public string ProductID { get; set; }
        [Display(Name = "Tên SP")]
        public string ProductName { get; set; }

        [Display(Name = "Loại SP")]
        public int Category { get; set; }

        [AllowHtml]
        [Display(Name = "Mô tả")]
        public string Description { get; set; }

        [AllowHtml]
        [Display(Name = "Thông số | cấu hình")]
        public string InfoPro { get; set; }

        [DisplayFormat(DataFormatString = "{0:#,##0.00}")]
        [Display(Name = "Giá bán SP")]
        public decimal Price { get; set; }
        
        [Display(Name = "Hình SP")]
        public string Image { get; set; }

        [Display(Name = "Hình SP bổ sung")]
        public string ImagePro { get; set; }

        [Display(Name = "Số lượng SP")]
        public int Quantity { get; set; }

        [Display(Name = "Màu sắc")]
        public string Color { get; set; }

        [Display(Name = "Kích thước")]
        public string Size { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageUpload { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageProItem { get; set; }
    }
}