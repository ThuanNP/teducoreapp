using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.Enums;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Blogs")]
    public class Blog : DomainEntity<int>, IHasSeoMetadata, ISwitchable, IDateTracking
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public string Content { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(255)]
        public string SeoPageTitle { get; set; }

        [StringLength(255)]
        public string SeoAlias { get; set; }

        [StringLength(255)]
        public string Seokeywords { get; set; }

        [StringLength(255)]
        public string SeoDecription { get; set; }

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
    }
}