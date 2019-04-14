using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("ShippingMethods")]
    public class ShippingMethod : DomainEntity<int>
    {
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Period { get; set; }
        public virtual ICollection<Bill> Bills { get; set; }
    }
}