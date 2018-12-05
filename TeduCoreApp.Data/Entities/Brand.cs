using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.Enums;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Brands")]
    public class Brand : DomainEntity<int>, ISwitchable
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        public Status Status { get; set; }
    }
}