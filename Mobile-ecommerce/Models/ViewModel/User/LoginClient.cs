using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.User
{
    public class LoginClient
    {
        public string login_name { set; get; }
      
        public string login_password { set; get; }
    }
}