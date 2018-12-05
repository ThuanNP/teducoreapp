﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("ProductCategories")]
    public class ProductCategory : DomainEntity<int>, IHasSeoMetadata, ISwitchable, ISortable, IDateTracking
    {
        public ProductCategory() => Products = new List<Product>();

        public ProductCategory(int id, string name, string description, int? parentId, int? homeOrder, string image, 
            bool? homeFlag, string seoPageTitle, string seoAlias, string seokeywords, 
            string seoDecription, Status status, int sortOrder, 
            DateTime dateCreated, DateTime dateModified, ICollection<Product> products)
        {
            Id = id;
            Name = name;
            Description = description;
            ParentId = parentId;
            HomeOrder = homeOrder;
            Image = image;
            HomeFlag = homeFlag;
            SeoPageTitle = seoPageTitle;
            SeoAlias = seoAlias;
            Seokeywords = seokeywords;
            SeoDecription = seoDecription;
            Status = status;
            SortOrder = sortOrder;
            DateCreated = dateCreated;
            DateModified = dateModified;
            Products = products;
        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Description { get; set; }

        public int? ParentId { get; set; }
        public int? HomeOrder { get; set; }

        [MaxLength(256)]
        public string Image { get; set; }

        public bool? HomeFlag { get; set; }

        [MaxLength(256)]
        public string SeoPageTitle { get; set; }

        [MaxLength(256)]
        public string SeoAlias { get; set; }

        [MaxLength(256)]
        public string Seokeywords { get; set; }

        [MaxLength(256)]
        public string SeoDecription { get; set; }

        public Status Status { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}