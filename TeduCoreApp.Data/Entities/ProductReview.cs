using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    public class ProductReview : DomainEntity<int>, ISwitchable, ISortable, IDateTracking
    {
        public int ProductId { get; set; }

        [Required, MaxLength(50)]
        public string NickName { get; set; }

        [Required, MaxLength(150)]
        public string Summary { get; set; }

        public string Content { get; set; }

        [Range(1, 5), DefaultValue(5)]
        public decimal PriceRate { get; set; }

        [Range(1, 5), DefaultValue(5)]
        public decimal QualityRate { get; set; }

        [Range(1, 5), DefaultValue(5)]
        public decimal ValueRate { get; set; }


        public Status Status { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual Product Product { get; set; }
    }
}
