using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Product
{
    public class Search
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
    }
}