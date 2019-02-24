using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Models.ProductViewModels;

namespace TeduCoreApp.Controllers
{
    public class ProductController : Controller
    {
        private const string catalogBodyClass = "shop_grid_full_width_page";
        private readonly IProductService productService;
        private readonly IProductCategoryService productCategoryService;
        private readonly IConfiguration configuration;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IConfiguration configuration)
        {
            this.productService = productService;
            this.productCategoryService = productCategoryService;
            this.configuration = configuration;
        }

        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("products/{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = catalogBodyClass;
            pageSize = pageSize ?? configuration.GetValue<int>("PageSize");
            var model = new CatalogViewModel
            {
                Data = productService.GetAllPaging(id, string.Empty, page, pageSize.Value),
                Category = productCategoryService.GetById(id),
                PageSize = pageSize.Value,
                SortType = sortBy
            };
            return View(model);
        }
    }
}