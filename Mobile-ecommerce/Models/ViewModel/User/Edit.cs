using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.User
{
    public class Edit
    {
        public int Id { get; set; }
        [Display(Name = "Tên tài khoản")]
        public string Name { get; set; }   
        public int Quyen { get; set; }
        public bool TrangThai { get; set; }
    }
}