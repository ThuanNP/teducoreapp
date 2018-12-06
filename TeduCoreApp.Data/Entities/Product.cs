using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Products")]
    public class Product : DomainEntity<int>, IHasSeoMetadata, ISwitchable, IDateTracking
    {
        public Product() => ProductTags = new List<ProductTag>();

        public Product(int id, string name, int categoryId, string thumbnailImage, decimal price, decimal? promotionPrice,
            decimal originalPrice, string description, string content,
            bool? homeFlag, bool? hotFlag, int? viewCount, string tags,
            string unit, ProductCategory productCategory, ICollection<ProductTag> productTags,
            Status status, string seoPageTitle, string seoAlias, string seokeywords,
            string seoDecription, DateTime dateCreated, DateTime dateModified)
        {
            Id = id;
            Name = name;
            CategoryId = categoryId;
            Image = thumbnailImage;
            Price = price;
            PromotionPrice = promotionPrice;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            ProductCategory = productCategory;
            ProductTags = productTags;
            Status = status;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            Seokeywords = seokeywords;
            SeoDecription = seoDecription;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Product(string name, int categoryId, string thumbnailImage, decimal price, decimal? promotionPrice, 
            decimal originalPrice, string description, string content, 
            bool? homeFlag, bool? hotFlag, int? viewCount, string tags, 
            string unit, ProductCategory productCategory, ICollection<ProductTag> productTags, 
            Status status, string seoPageTitle, string seoAlias, string seokeywords, 
            string seoDecription, DateTime dateCreated, DateTime dateModified)
        {
            Name = name;
            CategoryId = categoryId;
            Image = thumbnailImage;
            Price = price;
            PromotionPrice = promotionPrice;
            OriginalPrice = originalPrice;
            Description = description;
            Content = content;
            HomeFlag = homeFlag;
            HotFlag = hotFlag;
            ViewCount = viewCount;
            Tags = tags;
            Unit = unit;
            ProductCategory = productCategory;
            ProductTags = productTags;
            Status = status;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            Seokeywords = seokeywords;
            SeoDecription = seoDecription;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

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

        [ForeignKey("CategoryId")]
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual ICollection<ProductTag> ProductTags { set; get; }

        public Status Status { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256), Column(TypeName ="varchar(256)")]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        public string Seokeywords { get; set; }

        [MaxLength(256)]
        public string SeoDecription { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}