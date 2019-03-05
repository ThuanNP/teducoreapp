using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Models.ProductViewModels
{
    public class DetailViewModel
    {
        public ProductViewModel Product { get; set; }
        public ProductCategoryViewModel Category { get; set; }
        public bool Available { get; set; }
        public List<ProductViewModel> RelatedProducts { get; set; }
        public List<ProductViewModel> UpSellProducts { get; set; }
        public List<ProductViewModel> LatestProducts { get; set; }
        public List<TagViewModel> Tags { get; set; }
        public List<SelectListItem> Colors { get; set; }
        public List<SelectListItem> Sizes { get; set; }
    }
}