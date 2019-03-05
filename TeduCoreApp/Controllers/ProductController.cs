using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Models.ProductViewModels;
using static TeduCoreApp.Utilities.Constants.CommonConstants;

namespace TeduCoreApp.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService productService;
        private readonly ITagService tagService;
        private readonly IProductCategoryService productCategoryService;
        private readonly IConfiguration configuration;
        private readonly ICommonService commonService;

        public ProductController(IProductService productService, ITagService tagService, IProductCategoryService productCategoryService, IConfiguration configuration, ICommonService commonService)
        {
            this.productService = productService;
            this.tagService = tagService;
            this.productCategoryService = productCategoryService;
            this.configuration = configuration;
            this.commonService = commonService;
        }

        [HttpGet]
        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("{alias}-c.{id}.html", Name = "ProductByCategory")]
        public IActionResult Catalog(int id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = BodyCssClass.ProductCatalog;
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

        [HttpGet]
        [Route("{alias}-p.{id}.html", Name = "ProductDetail")]
        public IActionResult Details(int id)
        {
            ViewData["BodyClass"] = BodyCssClass.ProductDetail;
            var product = productService.GetById(id);
            product.ProductImages = productService.GetImages(id);
            product.ProductQuantities = productService.GetQuantities(id);
            product.WholePrices = productService.GetWholePrices(id);
            var tags = productService.GetTags(id);
            var colors = commonService.GetColors();
            var model = new DetailViewModel
            {
                Product = product,
                Category = productCategoryService.GetById(id: product.CategoryId),
                RelatedProducts = productService.GetTopRelated(id, top: 8),
                UpSellProducts = productService.GetTopSpecialOffers(top: 8),
                Tags = productService.GetTags(productId: id),
                Colors = commonService.GetColors().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList(),
                Sizes=commonService.GetSizes().Select(x=>new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                }).ToList()
            };
            return View(model);
        }

        [HttpGet]
        [Route("tag.{id}.html", Name = "ProductByTag")]
        public IActionResult Tagalog(string id, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = BodyCssClass.ProductCatalog;
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

        [HttpGet]
        [Route("search.html", Name = "ProductSearch")]
        public IActionResult Search(int? categoryId, string keyword, int? pageSize, string sortBy, int page = 1)
        {
            ViewData["BodyClass"] = BodyCssClass.ProductCatalog;
            pageSize = pageSize ?? configuration.GetValue<int>("PageSize");
            ProductCategoryViewModel category = new ProductCategoryViewModel();
            if (categoryId.HasValue && categoryId.Value != 0)
            {
                category = productCategoryService.GetById(categoryId.Value);
            }

            var model = new SearchResultViewModel
            {
                Data = productService.GetAllPaging(categoryId, keyword, page, pageSize.Value, sortBy),
                PageSize = pageSize.Value,
                SortType = sortBy,
                Keyword = string.IsNullOrEmpty(keyword) ? "All products" : keyword,
                Category = category
            };
            return View(model);
        }
    }
}