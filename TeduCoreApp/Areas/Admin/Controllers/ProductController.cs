using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        IProductCategoryService _productCategoryService;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            var model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            var model = _productService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _productService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public IActionResult SaveEntity(ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (productViewModel.Id == 0)
                {
                    _productService.Add(productViewModel);
                }
                else
                {
                    _productService.Update(productViewModel);
                }
                _productService.Save();
                return new OkObjectResult(productViewModel);
            }
            else
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(allErrors);
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (ModelState.IsValid)
            {
                _productService.Delete(id);
                _productService.Save();
                return new OkObjectResult(id);
            }
            else
            {
                return new BadRequestObjectResult(ModelState);
            }
        }
        #endregion
    }
}