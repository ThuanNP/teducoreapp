using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;

using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>
    {
        public Bill()
        {
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

        //[ForeignKey("CustomerId")]
        //public virtual AppUser User { set; get; }

        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}