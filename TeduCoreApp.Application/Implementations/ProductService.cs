using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.Utilities.Constants;
using TeduCoreApp.Utilities.Dtos;
using TeduCoreApp.Utilities.Helpers;
using static TeduCoreApp.Utilities.Constants.CommonConstants;

namespace TeduCoreApp.Application.Implementations
{
    public class ProductService : BaseService, IProductService
    {
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IProductRepository productRepository;
        private readonly ITagRepository tagRepository;
        private readonly IProductTagRepository productTagRepository;
        private readonly IProductQuantityRepository productQuantityRepository;
        private readonly IProductImageRepository productImageRepository;
        private readonly IWholePriceRepository wholePriceRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductService(IProductRepository productRepository, ITagRepository tagRepository,
            IProductCategoryRepository productCategoryRepository, IProductTagRepository productTagRepository,
            IProductQuantityRepository productQuantityRepository, IProductImageRepository productImageRepository,
            IWholePriceRepository wholePriceRepository, IUnitOfWork unitOfWork)
        {
            this.productCategoryRepository = productCategoryRepository;
            this.productRepository = productRepository;
            this.tagRepository = tagRepository;
            this.productTagRepository = productTagRepository;
            this.productQuantityRepository = productQuantityRepository;
            this.productImageRepository = productImageRepository;
            this.wholePriceRepository = wholePriceRepository;
            this.unitOfWork = unitOfWork;
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
                    if (!tagRepository.FindAll(tag => tag.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        tagRepository.Add(tag);
                    }
                    ProductTag productTag = new ProductTag
                    {
                        TagId = tagId
                    };
                    product.ProductTags.Add(productTag);
                }
            }
            productRepository.Add(product);
            return productViewModel;
        }

        public void AddQuantities(int productId, List<ProductQuantityViewModel> quantityViewModels)
        {
            var product = productRepository.FindById(productId, p => p.ProductQuantities);
            productQuantityRepository.RemoveMultiple(product.ProductQuantities.ToList());
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
            productRepository.Update(product);
        }

        public void Delete(int id) => productRepository.Remove(id);

        public List<ProductViewModel> GetAll()
        {
            return productRepository.FindAll(p => p.ProductCategory)
                .ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetAll(int? categoryId, string keyword)
        {
            var query = productRepository.FindAll(p => p.Status == Status.Active);
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

        public PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string SortBy)
        {
            var query = productRepository.FindAll(p => p.Status == Status.Active);
            if (categoryId.HasValue)
            {
                var subCategories = productCategoryRepository.FindAll(x => x.ParentId == categoryId);
                var CategoryIds = subCategories.Select(x => x.Id).ToList();
                query = query.Where(p => p.CategoryId == categoryId.Value || CategoryIds.Contains(p.CategoryId));
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
            switch (SortBy)
            {
                case ProductSortType.Latest:
                    query = query.OrderByDescending(p => p.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case ProductSortType.Name:
                    query = query.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case ProductSortType.PriceAsc:
                    query = query.OrderBy(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case ProductSortType.PriceDesc:
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                default:
                    query = query.OrderByDescending(p => p.PurchasedCount.HasValue).ThenByDescending(p => p.PurchasedCount).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
            }
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
            return Mapper.Map<Product, ProductViewModel>(productRepository.FindById(id, c => c.ProductTags));
        }

        public List<ProductQuantityViewModel> GetQuantities(int productId)
        {
            return productQuantityRepository.FindAll(x => x.ProductId == productId).ProjectTo<ProductQuantityViewModel>().ToList();
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

                        productRepository.Add(product);
                    }
                }
            }
        }

        public void Save() => unitOfWork.Commit();

        public void Update(ProductViewModel productViewModel)
        {
            var product = Mapper.Map<ProductViewModel, Product>(productViewModel);
            productTagRepository.RemoveMultiple(productTagRepository.FindAll(x => x.ProductId == productViewModel.Id).ToList());
            if (!string.IsNullOrEmpty(productViewModel.Tags))
            {
                string[] tags = productViewModel.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    // Add tags
                    if (!tagRepository.FindAll(tag => tag.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        tagRepository.Add(tag);
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
            productRepository.Update(product);
        }

        public List<ProductImageViewModel> GetImages(int productId)
        {
            return productImageRepository.FindAll(x => x.ProductId == productId).ProjectTo<ProductImageViewModel>().ToList();
        }

        public void AddImages(int productId, string[] images)
        {
            productImageRepository.RemoveMultiple(productImageRepository.FindAll(i => i.ProductId == productId).ToList());
            foreach (var image in images)
            {
                productImageRepository.Add(new ProductImage()
                {
                    Path = image,
                    ProductId = productId,
                    Caption = string.Empty
                });
            }
        }

        public List<WholePriceViewModel> GetWholePrices(int productId)
        {
            return wholePriceRepository.FindAll(x => x.ProductId == productId).ProjectTo<WholePriceViewModel>().ToList();
        }

        public void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices)
        {
            wholePriceRepository.RemoveMultiple(wholePriceRepository.FindAll(x => x.ProductId == productId).ToList());
            foreach (var wholePrice in wholePrices)
            {
                wholePriceRepository.Add(new WholePrice()
                {
                    ProductId = productId,
                    FromQuantity = wholePrice.FromQuantity,
                    ToQuantity = wholePrice.ToQuantity,
                    Price = wholePrice.Price
                });
            }
        }

        public List<ProductViewModel> GetTopSpecialOffers(int top)
        {
            var query = productRepository.FindAll(x => x.Status == Status.Active);
            return query.OrderByDescending(x => x.PurchasedCount.HasValue).OrderByDescending(x => (x.Price - (x.PromotionPrice ?? 0))).Take(top).ProjectTo<ProductViewModel>().ToList();

        }

        public async Task<List<ProductViewModel>> GetTopBestSellersAsync(int top)
        {
            var query = productRepository.FindAll(x => x.Status == Status.Active);
            return await query.OrderByDescending(x => x.PurchasedCount.HasValue).OrderByDescending(x => x.PurchasedCount.Value).Take(top).ProjectTo<ProductViewModel>().ToListAsync();
        }

        public List<ProductViewModel> GetTopBestSellers(int top)
        {
            var query = productRepository.FindAll(x => x.Status == Status.Active);
            return query.OrderByDescending(x => x.PurchasedCount.HasValue).OrderByDescending(x => x.PurchasedCount.Value).Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetTopNew(int top)
        {
            var query = productRepository.FindAll(x => x.Status == Status.Active);
            return query.OrderByDescending(x => x.DateCreated).Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetTopViewCount(int top)
        {
            var query = productRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true);
            return query.OrderByDescending(x => x.ViewCount).Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<ProductViewModel> GetTopRelated(int id, int top)
        {
            var product = productRepository.FindById(id);
            var query = productRepository.FindAll(x => x.Status == Status.Active && x.CategoryId == product.CategoryId);
            return query.OrderByDescending(x => x.DateCreated).Take(top).ProjectTo<ProductViewModel>().ToList();
        }

        public List<TagViewModel> GetTags(int productId)
        {
            var tags = tagRepository.FindAll();
            var productTags = productTagRepository.FindAll();
            var query = (from tag in tags
                         join ptag in productTags on tag.Id equals ptag.TagId
                         where ptag.ProductId == productId && tag.Type.Equals("Product")
                         select tag).ProjectTo<TagViewModel>();
            return query.ToList();
        }

        public PagedResult<ProductViewModel> GetAllTaging(string tagId, int page, int pageSize, string SortBy)
        {
            var query = productRepository.FindAll(p => p.Status == Status.Active);
            if (!string.IsNullOrEmpty(tagId))
            {
                var tags = tagRepository.FindAll();
                var productTags = productTagRepository.FindAll();
                query = from t in tags
                        join pt in productTags on t.Id equals pt.TagId
                        where t.Id.Equals(tagId) && t.Type.Equals("Product")
                        select pt.Product;
            }
            int totalRow = query.Count();
            switch (SortBy)
            {
                case ProductSortType.Latest:
                    query = query.OrderByDescending(p => p.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case ProductSortType.Name:
                    query = query.OrderBy(p => p.Name).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case ProductSortType.PriceAsc:
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
                case ProductSortType.PriceDesc:
                    query = query.OrderByDescending(x => x.PromotionPrice.HasValue ? x.PromotionPrice : x.Price).Skip((page - 1) * pageSize).Take(pageSize);
                    break;
            }

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

        public bool CheckAvailability(int productId, int sizeId, int colorId)
        {
            var quantity = productQuantityRepository.FindSingle(x => x.ColorId == colorId && x.SizeId == sizeId && x.ProductId == productId);
            return quantity == null ? false : quantity.Quantity > 0;
        }
    }
}
