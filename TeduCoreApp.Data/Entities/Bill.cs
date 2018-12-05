using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Bills")]
    public class Bill : DomainEntity<int>
    {
        public int? CustomerId { get; set; }

        [StringLength(255)]
        public string CustomerName { get; set; }

        [StringLength(500)]
        public string CustomerAddress { get; set; }

        [StringLength(50)]
        public string CustomerPhone { get; set; }

        [StringLength(255)]
        public string CustomerEmail { get; set; }

        public DateTime OrderDate { get; set; }

        [StringLength(10)]
        public string Status { get; set; }

        public virtual ICollection<BillDetail> BillDetails { get; set; }
    }
}