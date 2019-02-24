using System.ComponentModel.DataAnnotations;

namespace TeduCoreApp.Application.ViewModels.Common
{
    public class TagViewModel
    {
        public string Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }

        [Required, MaxLength(50)]
        public string Type { get; set; }
    }
}