using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mobile_ecommerce.Models.ViewModel.Voucher
{
    public class DetailVoucher
    {
        [Display(Name = "Mã khuyến mãi")]
        public string VoucherID { get; set; }
        [Display(Name = "Tên voucher")]
        public string Name { get; set; }

        [Display(Name = "Ngày bắt đầu")]
        public string DateStart { get; set; }

        [Display(Name = "Ngày kết thúc")]
        public string DateEnd { get; set; }

        [Display(Name = "Trị giá")]
        public decimal Value { get; set; }
    }
}