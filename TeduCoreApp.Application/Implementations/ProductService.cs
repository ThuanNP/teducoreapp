using AutoMapper;
using AutoMapper.QueryableExtensions;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.Utilities.Constants;
using TeduCoreApp.Utilities.Dtos;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Application.Implementations
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IProductTagRepository _productTagRepository;
        private IProductQuantityRepository _productQuantityRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, ITagRepository tagRepository, IProductTagRepository productTagRepository, IProductQuantityRepository productQuantityRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
            _productQuantityRepository = productQuantityRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductViewModel Add(ProductViewModel productViewModel)
        {
            productViewModel.SeoAlias = TextHelper.ToUnsignString(productViewModel.Name);
            var product = Mapper.Map<ProductViewModel, Product>(productViewModel);
            if (!string.IsNullOrEmpty(productViewModel.Tags))
            {
                string[] tags = productViewModel.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(tag => tag.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    product.ProductTags.Add(productTag);
                }
            }
            _productRepository.Add(product);
            return productViewModel;
        }

        public void AddQuantities(int productId, List<ProductQuantityViewModel> quantityViewModels)
        {
            var product = _productRepository.FindById(productId, p=>p.ProductQuantities);
            _productQuantityRepository.RemoveMultiple(product.ProductQuantities.ToList());           
            var quantityList = Mapper.Map<List<ProductQuantityViewModel>, List<ProductQuantity>>(quantityViewModels);
            List<ProductQuantity> newQuantityList = new List<ProductQuantity>();
            foreach (var item in quantityList)
            {
                var other = newQuantityList.FirstOrDefault(q => q.ProductId == item.ProductId && q.ColorId == item.ColorId && q.SizeId == item.SizeId);
                if (other != null)
                {
                    other.Quantity += item.Quantity;
                }
                else
                {
                    newQuantityList.Add(item);
                }
            }
            product.ProductQuantities = newQuantityList;
            _productRepository.Update(product);
        }

        public void Delete(int id) => _productRepository.Remove(id);

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(p => p.ProductCategory)
                .ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetAll(int? categoryId, string keyword)
        {
            var query = _productRepository.FindAll(p => p.Status == Status.Active);
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                             .OrderByDescending(p => p.DateCreated);
            }
            var data = query.ProjectTo<ProductViewModel>().ToList();
            return data;
        }

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize)
        {
            var query = _productRepository.FindAll(p => p.Status == Status.Active);
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.CategoryId == categoryId.Value);
            }
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                //query = query.Where(p => p.Name.Contains(keyword) ||
                //                    p.Description.Contains(keyword) ||
                //                    p.Seokeywords.Contains(keyword) ||
                //                    p.SeoDecription.Contains(keyword));
                query = query.Where(p => p.Name.Contains(keyword) ||
                                    p.Description.Contains(keyword));
            }
            int totalRow = query.Count();
            query = query.OrderByDescending(p => p.DateCreated)
                .Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.ProjectTo<ProductViewModel>().ToList();
            var paginationSet = new PagedResult<ProductViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public ProductViewModel GetById(int id)
        {
            return Mapper.Map<Product, ProductViewModel>(_productRepository.FindById(id));
        }

        public List<ProductQuantityViewModel> GetQuantities(int productId)
        {
            return _productQuantityRepository.FindAll(x => x.ProductId == productId).ProjectTo<ProductQuantityViewModel>().ToList();
        }

        public void ImportExcel(string filePath, int categoryId)
        {
            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[1];
                Product product;

                for (int i = worksheet.Dimension.Start.Row + 1; i < worksheet.Dimension.End.Row; i++)
                {
                    if (worksheet.Cells[i, 1].Value != null)
                    {
                        product = new Product();
                        decimal.TryParse((worksheet.Cells[i, 3].Value ?? string.Empty).ToString(), out var originalPrice);
                        decimal.TryParse((worksheet.Cells[i, 4].Value ?? string.Empty).ToString(), out var price);
                        decimal.TryParse((worksheet.Cells[i, 5].Value ?? string.Empty).ToString(), out var promotionPrice);
                        bool.TryParse((worksheet.Cells[i, 9].Value ?? string.Empty).ToString(), out var hotFlag);
                        bool.TryParse((worksheet.Cells[i, 10].Value ?? string.Empty).ToString(), out var homeFlag);

                        product.CategoryId = categoryId;
                        product.Name = (worksheet.Cells[i, 1].Value ?? string.Empty).ToString();
                        product.SeoAlias = TextHelper.ToUnsignString(product.Name);
                        product.Description = (worksheet.Cells[i, 2].Value ?? string.Empty).ToString();
                        product.Price = originalPrice;
                        product.Price = price;
                        product.PromotionPrice = promotionPrice;
                        product.Content = (worksheet.Cells[i, 6].Value ?? string.Empty).ToString();
                        product.SeoKeywords = (worksheet.Cells[i, 7].Value ?? string.Empty).ToString();
                        product.SeoDescription = (worksheet.Cells[i, 8].Value ?? string.Empty).ToString();
                        product.HotFlag = hotFlag;
                        product.HomeFlag = homeFlag;
                        product.Status = Status.Active;

                        _productRepository.Add(product);
                    }
                }
            }
        }

        public void Save()
        {
            _unitOfWork.Commit();
        }

        public void Update(ProductViewModel productViewModel)
        {
            var product = Mapper.Map<ProductViewModel, Product>(productViewModel);
            if (!string.IsNullOrEmpty(productViewModel.Tags))
            {
                _productTagRepository.RemoveMultiple(_productTagRepository.FindAll(x => x.ProductId == productViewModel.Id).ToList());
                string[] tags = productViewModel.Tags.Split(',');
                var productTags = _productTagRepository.FindAll(x => x.ProductId == productViewModel.Id).ToList();
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    // Add tags
                    if (!_tagRepository.FindAll(tag => tag.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    // Add product tags 
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    product.ProductTags.Add(productTag);
                }
            }
            product.SeoAlias = TextHelper.ToUnsignString(productViewModel.Name);
            _productRepository.Update(product);
        }
    }
}
