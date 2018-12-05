using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("BlogTags")]
    public class BlogTag : DomainEntity<string>
    {
        public int BlogId { get; set; }

        [Required, MaxLength(50), Column(TypeName = "varchar")]
        public string TagId { get; set; }

        [ForeignKey("BlogId")]
        public virtual Blog Blog { get; set; }

        [ForeignKey("TagId")]
        public virtual Tag Tag { get; set; }
    }
}