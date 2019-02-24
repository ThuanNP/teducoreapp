using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Data.Enums;

namespace TeduCoreApp.Application.ViewModels.Blog
{
    public class BlogViewModel
    {
        public int Id { get; set; }

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

        public List<BlogTagViewModel> BlogTags { get; set; }
    }
}