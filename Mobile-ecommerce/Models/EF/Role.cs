using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public string Des { get; set; }

        public ICollection<User> Users { get; set; }
    }
}