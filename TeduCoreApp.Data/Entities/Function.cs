using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Functions")]
    public class Function : DomainEntity<string>, ISwitchable, ISortable
    {
        public Function()
        {
        }

        public Function(string id, string name, string url, string parentId, string iconCss, int sortOrder)
        {
            Id = id;
            Name = name;
            URL = url;
            ParentId = parentId;
            IconCss = iconCss;
            SortOrder = sortOrder;
            Status = Status.Active;
        }

        public Function(string name, string url, string parentId, string iconCss, int sortOrder)
        {
            Name = name;
            URL = url;
            ParentId = parentId;
            IconCss = iconCss;
            SortOrder = sortOrder;
            Status = Status.Active;
        }

        [Required, MaxLength(128)]
        public string Name { set; get; }

        [Required, MaxLength(250)]
        public string URL { set; get; }

        [MaxLength(128)]
        public string ParentId { set; get; }

        public string IconCss { get; set; }
        public int SortOrder { set; get; }
        public Status Status { set; get; }
    }
}