using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Brands")]
    public class Brand : DomainEntity<int>, ISwitchable
    {
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        public Status Status { get; set; }
    }
}