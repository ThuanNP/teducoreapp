using System.ComponentModel.DataAnnotations;

namespace TeduCoreApp.Application.ViewModels.Common
{
    public class SlideViewModel
    {
        public int Id { get; set; }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(250)]
        public string Description { set; get; }

        [MaxLength(250)]
        public string Url { get; set; }

        public int? DisplayOrder { set; get; }

        public bool Status { set; get; }

        public string Content { set; get; }

        [Required, MaxLength(250)]
        public string Image { get; set; }

        [Required, MaxLength(25)]
        public string GroupAlias { get; set; }
    }
}