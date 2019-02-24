using System.ComponentModel.DataAnnotations;

namespace TeduCoreApp.Application.ViewModels.Common
{
    public class FooterViewModel
    {
        public string Id { get; set; }

        [Required]
        public string Content { get; set; }
    }
}