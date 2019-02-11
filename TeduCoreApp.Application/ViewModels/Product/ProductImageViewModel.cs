using System.ComponentModel.DataAnnotations;

namespace TeduCoreApp.Application.ViewModels.Product
{
    public class ProductImageViewModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        [MaxLength(250)]
        public string Path { get; set; }

        [MaxLength(250)]
        public string Caption { get; set; }
    }
}
