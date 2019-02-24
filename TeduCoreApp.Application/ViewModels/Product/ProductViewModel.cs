using System;
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
        [Description("Category ID")]
        public int CategoryId { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        [Required, DefaultValue(0)]
        public decimal Price { get; set; }

        [Description("Promotion price")]
        public decimal? PromotionPrice { get; set; }

        [Required, DefaultValue(0)]
        [Description("Original price")]
        public decimal OriginalPrice { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public string Content { get; set; }

        [Description("It's shown on homepage")]
        public bool? HomeFlag { get; set; }

        [Description("It's a hot item")]
        public bool? HotFlag { get; set; }

        [Description("View count")]
        public int? ViewCount { get; set; }

        [Description("Purchased count")]
        public int? PurchasedCount { get; set; }

        [MaxLength(256)]
        public string Tags { get; set; }

        [MaxLength(256)]
        public string Unit { get; set; }

        public Status Status { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256)]
        [Description("SEO alias")]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        [Description("SEO keyword")]
        public string SeoKeywords { get; set; }

        [MaxLength(256)]
        [Description("SEO description")]
        public string SeoDescription { get; set; }

        [Description("Created on date")]
        public DateTime DateCreated { get; set; }

        [Description("Modified on date")]
        public DateTime DateModified { get; set; }

        [Description("Product category")]
        public ProductCategoryViewModel ProductCategory { get; set; }
    }
}