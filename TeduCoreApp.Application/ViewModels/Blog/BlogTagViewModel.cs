using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Application.ViewModels.Common;

namespace TeduCoreApp.Application.ViewModels.Blog
{
    public class BlogTagViewModel
    {
        public string Id { get; set; }

        public int BlogId { get; set; }

        [Required, MaxLength(50)]
        public string TagId { get; set; }
    }
}