using System;
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

        public Blog(int id, string name, string description, string content, string image, string seoPageTitle, string seoAlias, string seokeywords, string seoDecription, Status status, DateTime dateCreated, DateTime dateModified)
        {
            Id = id;
            Name = name;
            Description = description;
            Content = content;
            Image = image;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            Seokeywords = seokeywords;
            SeoDecription = seoDecription;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        public Blog(string name, string description, string content, string image, string seoPageTitle, string seoAlias, string seokeywords, string seoDecription, Status status, DateTime dateCreated, DateTime dateModified)
        {
            Name = name;
            Description = description;
            Content = content;
            Image = image;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            Seokeywords = seokeywords;
            SeoDecription = seoDecription;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public string Content { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256)]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        public string Seokeywords { get; set; }

        [MaxLength(256)]
        public string SeoDecription { get; set; }

        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}