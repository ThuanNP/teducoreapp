using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("SystemConfigs")]
    public class SystemConfig : DomainEntity<int>
    {
        [StringLength(255)]
        public string Value1 { get; set; }

        public int? Value2 { get; set; }
        public decimal? Value3 { get; set; }
        public bool? Value4 { get; set; }
    }
}