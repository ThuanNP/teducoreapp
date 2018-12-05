using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Slides")]
    public class Slides : DomainEntity<int>
    {
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { set; get; }

        [MaxLength(250)]
        public string Url { get; set; }

        public int? DisplayOrder { set; get; }

        public bool Status { set; get; }

        public string Content { set; get; }

        [Required, MaxLength(250)]
        public string Image { get; set; }

        [Required, MaxLength(25)]
        public string GroupAlias { get; set; }
    }
}