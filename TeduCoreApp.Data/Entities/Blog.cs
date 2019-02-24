using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Blogs")]
    public class Blog : DomainEntity<int>, IHasSeoMetadata, ISwitchable, IDateTracking
    {
        public Blog()
        {
        }

        public Blog(int id, string name, string description, string content, string image, string seoPageTitle, string seoAlias, string seokeywords, string seoDecription, Status status)
        {
            Id = id;
            Name = name;
            Description = description;
            Content = content;
            Image = image;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seokeywords;
            SeoDescription = seoDecription;
            Status = status;
        }

        public Blog(string name, string description, string content, string image, string seoPageTitle, string seoAlias, string seokeywords, string seoDecription, Status status)
        {
            Name = name;
            Description = description;
            Content = content;
            Image = image;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            SeoKeywords = seokeywords;
            SeoDescription = seoDecription;
            Status = status;
        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string Content { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        public bool? HomeFlag { set; get; }
        public bool? HotFlag { set; get; }
        public int? ViewCount { set; get; }

        public string Tags { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256)]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        public string SeoKeywords { get; set; }

        [MaxLength(256)]
        public string SeoDescription { get; set; }

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<BlogTag> BlogTags { get; set; }
    }
}