using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Blog;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            LatestBlogs = new List<BlogViewModel>();
            HomeSlides = new List<SlideViewModel>();
            ProductsTopViewCount = new List<ProductViewModel>();
            NewProducts= new  List<ProductViewModel>();
            SpecialProductOffers = new List<ProductViewModel>();
            HomeCategories = new List<ProductCategoryViewModel>();
        }

        public List<BlogViewModel> LatestBlogs { get; set; }
        public List<SlideViewModel> HomeSlides { get; set; }
        public List<ProductViewModel> ProductsTopViewCount { get; set; }
        public List<ProductViewModel> NewProducts { get; set; }
        public List<ProductViewModel> SpecialProductOffers { get; set; }
        public List<ProductCategoryViewModel> HomeCategories { get; set; }
        public string Title { get; set; }
        public string MetaKeyword { get; set; }
        public string MetaDescription { get; set; }
    }
}