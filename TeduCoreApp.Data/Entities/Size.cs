using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Sizes")]
    public class Size : DomainEntity<int>
    {
        public Size()
        {
        }

        public Size(string name)
        {
            Name = name;
        }

        [MaxLength(250)]
        public string Name { get; set; }
    }
}