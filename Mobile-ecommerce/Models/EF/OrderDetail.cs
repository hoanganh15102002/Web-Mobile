using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class OrderDetail
    {
        [Key]       
        public int OrderDetailID { get; set; }
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalMoney { get; set; }

        public int ProductID { get; set; }
        public ICollection<Product> Products { get; set; }

        public int OrderID { get; set; }
        public Order Orders { get; set; }    
    }
}