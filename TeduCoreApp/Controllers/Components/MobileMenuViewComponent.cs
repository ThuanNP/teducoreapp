using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;

namespace TeduCoreApp.Controllers.Components
{
    public class MobileMenuViewComponent : ViewComponent
    {
        private readonly IProductCategoryService productCategoryService;
        private readonly IProductService productService;

        public MobileMenuViewComponent(IProductCategoryService productCategoryService, IProductService productService)
        {
            this.productCategoryService = productCategoryService;
            this.productService = productService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.BestSellers = productService.GetTopBestSellers(3);
            return View(productCategoryService.GetAll());
        }
    }
}
