using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Pages")]
    public class Page : DomainEntity<string>
    {
        public Page()
        {
        }

        public Page(string id, string name, string content)
        {
            Id = id;
            Name = name;
            Content = content;
        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        public string Content { get; set; }
    }
}