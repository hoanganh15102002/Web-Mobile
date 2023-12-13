using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.User
{
    public class UserRegister
    {
        public string RegisterName { get; set; }
        public string RegisterEmail { get; set; }
        public string RegisterPass { get; set; }
        public string ConfirmPass { get; set; }
    }
}