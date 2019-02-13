using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Utilities.Dtos;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private readonly IProductService _productService;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IHostingEnvironment _hostingEnvironment;

        public ProductController(IProductService productService, IProductCategoryService productCategoryService, IHostingEnvironment hostingEnvironment)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region AJAX API

        [HttpGet]
        public IActionResult GetAllCategories()
        {
            List<ProductCategoryViewModel> model = _productCategoryService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetById(int id)
        {
            ProductViewModel model = _productService.GetById(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ProductViewModel> model = _productService.GetAll();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            PagedResult<ProductViewModel> model = _productService.GetAllPaging(categoryId, keyword, page, pageSize);
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

        [HttpPost]
        public IActionResult ImportExcel(IList<IFormFile> files, int categoryid)
        {
            if (files != null && files.Count > 0)
            {
                IFormFile file = files[0];
                string filename = ContentDispositionHeaderValue
                                .Parse(file.ContentDisposition)
                                .FileName.Trim('"');
                string directory = $@"{_hostingEnvironment.WebRootPath}\uploaded\excels";
                //Create directory if it isn't existing
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                string filePath = Path.Combine(directory, filename);
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
                _productService.ImportExcel(filePath, categoryid);
                _productService.Save();
                return new OkObjectResult(filePath);
            }
            return new NoContentResult();
        }

        public IActionResult ExportExcel(int? categoryId, string keyword)
        {
            string webRootFolder = _hostingEnvironment.WebRootPath;
            const string folder = "export-files";
            string directory = Path.Combine(webRootFolder, folder);
            //Create directory if it isn't existing
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            string fileName = $"Products";
            if (categoryId.HasValue)
            {
                var productCategory =  _productCategoryService.GetById(categoryId.Value);
                fileName += $"_{TextHelper.ToUnsignString(productCategory.Name)}";
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                fileName += $"_{TextHelper.ToUnsignString(keyword)}";
            }
            fileName += $"_{ DateTime.Now:yyyyMMddhhmmss}.xlsx";
            string fileUrl = $"{Request.Scheme}://{Request.Host}/{folder}/{fileName}";
            FileInfo file = new FileInfo(Path.Combine(directory, fileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(webRootFolder, fileName));
            }
            List<ProductViewModel> products = _productService.GetAll(categoryId, keyword);
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Products");
                worksheet.Cells["A1"].LoadFromCollection(products, true, TableStyles.Light1);
                //Format columns
                const string integerFormat = "#,##0";
                worksheet.Column(1).Style.Numberformat.Format = integerFormat;

                //number with 2 decimal places and thousand separator and money symbol
                const string currencyFormat = "#,##0 ₫";
                worksheet.Column(5).Style.Numberformat.Format = currencyFormat;
                worksheet.Column(6).Style.Numberformat.Format = currencyFormat;
                worksheet.Column(7).Style.Numberformat.Format = currencyFormat;

                const string dateTimeFormat = "dd-MM-yyyy HH:mm:ss";
                worksheet.Column(20).Style.Numberformat.Format = dateTimeFormat;
                worksheet.Column(21).Style.Numberformat.Format = dateTimeFormat;
                worksheet.DeleteColumn(22);
                worksheet.Cells.AutoFitColumns();
                package.Save(); //Save the workbook.
            }
            return new OkObjectResult(fileUrl);
        }

        [HttpGet]
        public IActionResult GetQuantities(int productId)
        {
            var quantities = _productService.GetQuantities(productId);
            return new OkObjectResult(quantities);
        }

        [HttpPost]
        public IActionResult SaveQuantities(int productId, List<ProductQuantityViewModel> quantityViewModels)
        {
            _productService.AddQuantities(productId, quantityViewModels);
            _productService.Save();
            return new OkObjectResult(quantityViewModels);
        }

        [HttpGet]
        public IActionResult GetImages(int productId)
        {
            var images = _productService.GetImages(productId);
            return new OkObjectResult(images);
        }

        [HttpPost]
        public IActionResult SaveImages(int productId, string[] images)
        {
            _productService.AddImages(productId, images);
            _productService.Save();
            return new OkObjectResult(images);
        }

        [HttpGet]
        public IActionResult GetWholePrices(int productId)
        {
            var wholePrices = _productService.GetWholePrices(productId);
            return new OkObjectResult(wholePrices);
        }

        [HttpPost]
        public IActionResult SaveWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            _productService.AddWholePrice(productId, wholePrices);
            _productService.Save();
            return new OkObjectResult(wholePrices);
        }
        #endregion AJAX API
    }
}