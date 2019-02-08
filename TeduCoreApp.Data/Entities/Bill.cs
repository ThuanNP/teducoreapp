using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public Bill()
        {           
            BillDetails = new List<BillDetail>();
        }

        public Bill(Guid? customerId, string customerName, string customerAddress, string customerMobile, string customerMessage, string customerEmail, 
            BillStatus billStatus, PaymentMethod paymentMethod, Status status)
        {
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerEmail = customerEmail;
            CustomerMessage = customerMessage;
            BillStatus = billStatus;
            PaymentMethod = paymentMethod;
            Status = status;
            CustomerId = customerId;
            BillDetails = new List<BillDetail>();
        }

        public Bill(int id, Guid? customerId, string customerName, string customerAddress, string customerMobile, string customerEmail, 
            string customerMessage, BillStatus billStatus, PaymentMethod paymentMethod, Status status)
        {
            CustomerId = customerId;
            CustomerName = customerName;
            CustomerAddress = customerAddress;
            CustomerMobile = customerMobile;
            CustomerEmail = customerEmail;
            CustomerMessage = customerMessage;
            PaymentMethod = paymentMethod;
            BillStatus = billStatus;
            Status = status;
            BillDetails = new List<BillDetail>();
        }

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

        [DefaultValue(Status.Active)]
        public Status Status { get; set; } = Status.Active;

        [ForeignKey("CustomerId")]
        public virtual AppUser User { set; get; }

        public virtual ICollection<BillDetail> BillDetails { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}