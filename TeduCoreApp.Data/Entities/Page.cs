using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Pages")]
    public class Page : DomainEntity<string>
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        public string Content { get; set; }
    }
}