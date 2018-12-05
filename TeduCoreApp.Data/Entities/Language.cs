using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Languages")]
    public class Language : DomainEntity<int>
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        public bool Default { get; set; }
    }
}