using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Data.Enums;

namespace TeduCoreApp.Application.ViewModels.Product
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        [Required, DefaultValue(0)]
        public decimal Price { get; set; }

        public decimal? PromotionPrice { get; set; }

        [Required, DefaultValue(0)]
        public decimal OriginalPrice { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public string Content { get; set; }
        public bool? HomeFlag { get; set; }
        public bool? HotFlag { get; set; }
        public int? ViewCount { get; set; }

        [MaxLength(256)]
        public string Tags { get; set; }

        [MaxLength(256)]
        public string Unit { get; set; }

        public Status Status { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256)]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        public string SeoKeywords { get; set; }

        [MaxLength(256)]
        public string SeoDescription { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public ProductCategoryViewModel ProductCategory { get; set; }
    }
}