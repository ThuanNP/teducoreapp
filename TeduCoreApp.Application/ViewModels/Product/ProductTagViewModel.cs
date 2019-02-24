using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Application.ViewModels.Common;

namespace TeduCoreApp.Application.ViewModels.Product
{
    public class ProductTagViewModel
    {
        public int ProductId { get; set; }

        [Required, MaxLength(50)]
        public string TagId { get; set; }
        
        public virtual ProductViewModel Product { get; set; }
        
        public virtual TagViewModel Tag { get; set; }
    }
}