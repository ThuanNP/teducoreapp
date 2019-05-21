using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.Enums;

namespace TeduCoreApp.Application.ViewModels.Product
{
    public class BillViewModel
    {

        public int Id { get; set; }

        public Guid? CustomerId { get; set; }

        [Required, MaxLength(256)]
        public string CustomerName { get; set; }

        [Required, MaxLength(256)]
        public string CustomerAddress { get; set; }

        [Required, MaxLength(256)]
        public string CustomerMobile { get; set; }

        [MaxLength(256)]
        public string CustomerEmail { get; set; }

        [MaxLength(256)]
        public string CustomerMessage { set; get; }

        public DateTime OrderDate { get; set; }

        public PaymentMethod PaymentMethod { set; get; }

        public BillStatus BillStatus { set; get; }

        public Status Status { get; set; }

        public int ShippingMethodId { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public List<BillDetailViewModel> BillDetails { get; set; }
    }
}
