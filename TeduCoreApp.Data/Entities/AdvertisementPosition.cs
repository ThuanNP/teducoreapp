using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("AdvertisementPositions")]
    public class AdvertisementPosition : DomainEntity<string>
    {
        public string PageId { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [ForeignKey("PageId")]
        public virtual AdvertisementPage AdvertisementPage { get; set; }
    }
}