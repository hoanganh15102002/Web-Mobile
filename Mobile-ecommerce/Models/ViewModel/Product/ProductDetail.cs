using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Product
{
    public class ProductDetail
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string Category { get; set; }
   
        public string Price { get; set; }
        public string Image { get; set; }
        public int Quantity { get; set; }    
        public string Color { get; set; }
    }
}