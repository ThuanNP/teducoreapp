using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Models;
using static TeduCoreApp.Utilities.Constants.CommonConstants;

namespace TeduCoreApp.Controllers
{
    public class HomeController : Controller
    {
        private const string bodyClass = "cms-index-index cms-home-page";
        private readonly ICommonService commonService;
        private readonly IProductService productService;
        private readonly IProductCategoryService productCategoryService;
        private readonly IBlogService blogService;

        public HomeController(ICommonService commonService, IProductService productService, IProductCategoryService productCategoryService, IBlogService blogService)
        {
            this.commonService = commonService;
            this.productService = productService;
            this.productCategoryService = productCategoryService;
            this.blogService = blogService;
        }

        [Route("index.html")]
        [Route("")]
        public IActionResult Index()
        {
            ViewData["BodyClass"] = bodyClass;
            var model = new HomeViewModel
            {
                HomeSlides = commonService.GetSlides(SlideGroupAlias.Top, 4),
                LatestBlogs = blogService.GetLatest(8),
                SpecialProductOffers = productService.GetTopSpecialOffers(8),
                HomeCategories = productCategoryService.GetHomeCategories(4, 8),                
                NewProducts = productService.GetTopNew(6),
                ProductsTopViewCount = productService.GetTopViewCount(6)
            };
            return View(model);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
