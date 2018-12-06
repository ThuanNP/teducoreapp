using System;
using System.Collections.Generic;
using TeduCoreApp.Data.Enums;

namespace TeduCoreApp.Application.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoryId { get; set; }
        public string Image { get; set; }
        public decimal Price { get; set; }
        public decimal? PromotionPrice { get; set; }
        public decimal OriginalPrice { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool? HomeFlag { get; set; }
        public bool? HotFlag { get; set; }
        public int? ViewCount { get; set; }
        public string Tags { get; set; }
        public string Unit { get; set; }
        public Status Status { get; set; }
        public string SeoPageTitle { get; set; }
        public string SeoAlias { get; set; }
        public string Seokeywords { get; set; }
        public string SeoDecription { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public ProductCategoryViewModel Productcategory { get; set; }
        public ICollection<ProductTagViewModel> ProductTags { set; get; }
    }
}