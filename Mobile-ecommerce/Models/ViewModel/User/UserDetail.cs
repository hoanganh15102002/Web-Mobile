using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.User
{
    [Serializable]
    public class UserDetail
    {
        public int Id { get; set; }
        public string Name { get; set; }         
        public string Quyen { get; set; }
        public bool TrangThai { get; set; }  
    }
}