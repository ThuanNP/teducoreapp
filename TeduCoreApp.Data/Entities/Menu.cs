using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.Enums;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Menus")]
    public class Menu : DomainEntity<int>, ISwitchable, ISortable
    {
        [Required, StringLength(255)]
        public string Name { get; set; }

        [StringLength(255)]
        public string Url { get; set; }

        [StringLength(255)]
        public string Image { get; set; }

        [StringLength(255)]
        public string Css { get; set; }

        public Status Status { get; set; }
        public int SortOrder { get; set; }
    }
}