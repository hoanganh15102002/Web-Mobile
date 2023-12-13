using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string UserPass { get; set; }
        public bool Status { get; set; }

        public int RoleID { get; set; }
        public Role Role { get; set; }

   
        public virtual Customer Customer { get; set; }    

    }
}