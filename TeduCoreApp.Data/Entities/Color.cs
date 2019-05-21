using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Colors")]
    public class Color : DomainEntity<int>
    {
        public Color()
        {
        }

        public Color(string name, string code)
        {
            Name = name;
            Code = code;
        }

        [MaxLength(250)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Code { get; set; }
    }
}