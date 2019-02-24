using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Models.ProductViewModels;

namespace TeduCoreApp.Controllers
{
    public class ProductController : Controller
    {
        private const string catalogBodyClass = "shop_grid_full_width_page";
        private const string detailBodyClass = "product-page";
        private readonly IProductService productService;
        private readonly ITagService tagService;
        private readonly IProductCategoryService productCategoryService;
        private readonly IConfiguration configuration;

        public ProductController(IProductService productService, ITagService tagService, IProductCategoryService productCategoryService, IConfiguration configuration)
        {
            this.productService = productService;
            this.tagService = tagService;
            this.productCategoryService = productCategoryService;
            this.configuration = configuration;
        }

        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("products/{alias}-c.{id}.html", Name ="ProductByCategory")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = catalogBodyClass;
            pageSize = pageSize ?? configuration.GetValue<int>("PageSize");
            var model = new CatalogViewModel
            {
                Data = productService.GetAllPaging(id, string.Empty, page, pageSize.Value, sortBy),
                Category = productCategoryService.GetById(id),
                PageSize = pageSize.Value,
                SortType = sortBy
            };
            return View(model);
        }

        [Route("products/{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = detailBodyClass;
            var product = productService.GetById(id);
            product.ProductImages = productService.GetImages(id);
            product.ProductQuantities = productService.GetQuantities(id);
            product.WholePrices = productService.GetWholePrices(id);
            var category = productCategoryService.GetById(product.CategoryId);
            var relatedProducts = productService.GetTopRelated(id, 8);
            var upSellProducts = productService.GetTopSpecialOffers(8);
            var tags = productService.GetTags(id);
            var model = new DetailViewModel
            {
                Product = product,
                Category = category,
                RelatedProducts = relatedProducts,
                UpSellProducts = upSellProducts,
                Tags = tags
            };
            return View(model);
        }

        [Route("products/tag.{id}.html", Name = "ProductByTag")]
        public IActionResult Tagalog(string id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = catalogBodyClass;
            pageSize = pageSize ?? configuration.GetValue<int>("PageSize");
            var model = new TagalogViewModel
            {
                Data = productService.GetAllTaging(id, page, pageSize.Value, sortBy),
                Tag = tagService.GetById(id),
                PageSize = pageSize.Value,
                SortType = sortBy
            };
            return View(model);
        }
    }
}