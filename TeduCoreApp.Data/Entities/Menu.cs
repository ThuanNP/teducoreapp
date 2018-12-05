using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Menus")]
    public class Menu : DomainEntity<int>, ISwitchable, ISortable
    {
        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        [MaxLength(256)]
        public string Css { get; set; }

        public Status Status { get; set; }
        public int SortOrder { get; set; }
    }
}