using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;

namespace TeduCoreApp.Application.ViewModels.Product
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public int? HomeOrder { get; set; }
        public string Image { get; set; }
        public bool? HomeFlag { get; set; }
        public string SeoPageTitle { get; set; }
        public string SeoAlias { get; set; }
        public string Seokeywords { get; set; }
        public string SeoDecription { get; set; }
        public Status Status { get; set; }
        public int SortOrder { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }

        public List<ProductViewModel> Products { get; set; }

        public List<ProductCategoryViewModel> Children { get; set; }

        [ForeignKey("ParentId")]
        public ProductCategoryViewModel Parent { get; set; }
    }
}