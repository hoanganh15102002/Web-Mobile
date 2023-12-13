using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.EF
{
    public class Voucher
    {
        [Key]
        public string VoucherID { get; set; }
        public string VoucherName { get; set; }
        public string VouDes { get; set; }
        public decimal Discount { get; set; }       
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

    }
}