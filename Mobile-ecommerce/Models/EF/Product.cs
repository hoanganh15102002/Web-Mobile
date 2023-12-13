using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class Product
    {
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ProDes { get; set; }
        public string Images { get; set; }
        public string ImageNote { get; set; }
        public int Quantity { get; set; }
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal Price { get; set; }         
        public string Color { get; set; }
        public string InfoDes { get; set; }
      
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        public ICollection<ReviewPro> ReviewPros { get; set; }
        public OrderDetail OrderDetail { get; set; }
    }
}