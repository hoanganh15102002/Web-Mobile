using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.User
{
    public class Create
    {
        [Required(ErrorMessage = "Bạn chưa nhập tên tài khoản")]
        [Display(Name = "Tên tài khoản")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập mật khẩu")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Bạn chưa nhập lại mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận chưa đúng")]
        public string Password_Confirm { get; set; }
        public int Quyen { get; set; }
        public bool TrangThai { get; set; }
      
    }
}