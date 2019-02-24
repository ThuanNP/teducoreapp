using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public DetailViewModel()
        {
            Category = new ProductCategoryViewModel();
            Product = new ProductViewModel();
            RelatedProducts = new List<ProductViewModel>();
            UpSellProducts = new List<ProductViewModel>();
            LatestProducts = new List<ProductViewModel>();
        }

        public ProductCategoryViewModel Category { get; set; }
        public ProductViewModel Product { get; set; }
        public List<ProductViewModel> RelatedProducts { get; set; }
        public List<ProductViewModel> UpSellProducts { get; set; }
        public List<ProductViewModel> LatestProducts { get; set; }
        public List<TagViewModel> Tags { get; set; }
    }
}
