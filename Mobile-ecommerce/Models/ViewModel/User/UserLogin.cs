using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.User
{
    public class UserLogin
    {
        public int UserID { get; set; }
        public string username { get; set; }
        public string Role { get; set; }
        public string CusName { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}