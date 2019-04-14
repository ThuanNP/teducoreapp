using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;

namespace TeduCoreApp.Controllers.Components
{
    public class CategorySearchViewComponent : ViewComponent
    {
        private readonly IProductCategoryService productCategoryService;

        public CategorySearchViewComponent(IProductCategoryService productCategoryService)
        {
            this.productCategoryService = productCategoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await productCategoryService.GetAllAsync());
        }
    }
}
