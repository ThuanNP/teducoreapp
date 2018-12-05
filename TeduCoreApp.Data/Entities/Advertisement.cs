using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Advertisements")]
    public class Advertisement : DomainEntity<int>, ISwitchable
    {
        [MaxLength(20)]
        public string PositionId { get; set; }

        [MaxLength(256)]
        public string Url { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        public Status Status { get; set; }

        [ForeignKey("PositionId")]
        public virtual AdvertisementPosition AdvertisementPosition { get; set; }
    }
}