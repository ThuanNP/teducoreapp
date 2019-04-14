using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;

namespace TeduCoreApp.Application.Implementations
{
    public class ProductCategoryService : BaseService, IProductCategoryService
    {
        private readonly IProductCategoryRepository productCategoryRepository;
        private readonly IProductRepository productRepository;
        private readonly IUnitOfWork unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IProductRepository productRepository, IUnitOfWork unitOfWork)
        {
            this.productCategoryRepository = productCategoryRepository;
            this.productRepository = productRepository;
            this.unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productcategoryVm)
        {
            ProductCategory productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productcategoryVm);
            productCategoryRepository.Add(productCategory);
            return productcategoryVm;
        }

        public void Delete(int id)
        {
            var category = productCategoryRepository.FindById(id);
            var sibling = productCategoryRepository.FindAll(x => x.ParentId == category.ParentId
                           && x.SortOrder > category.SortOrder && x.Id != category.Id);
            // update followed siblings from category
            int order = category.SortOrder;
            foreach (var item in sibling)
            {
                item.SortOrder = ++order;
                productCategoryRepository.Update(item);
            }
            productCategoryRepository.Remove(id);
        }

        public async Task<List<ProductCategoryViewModel>> GetAllAsync()
        {
            return await productCategoryRepository.FindAll()
                .OrderBy(x => x.ParentId)
                .ProjectTo<ProductCategoryViewModel>().ToListAsync();
        }

        public List<ProductCategoryViewModel> GetAll()
        {
            return productCategoryRepository.FindAll()
                .OrderBy(x => x.ParentId)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId)
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            else
                return productCategoryRepository.FindAll()
                   .OrderBy(x => x.ParentId)
                   .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return productCategoryRepository.FindAll(x => x.Status == Status.Active && x.ParentId == parentId)
                .OrderBy(x => x.SortOrder)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(productCategoryRepository.FindById(id, c => c.Parent));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int topCategory, int topProduct)
        {
            var query = productCategoryRepository.FindAll(x => x.HomeFlag == true, c => c.Children)
                  .OrderBy(x => x.HomeOrder)
                  .Take(topCategory).ProjectTo<ProductCategoryViewModel>();

            var categories = query.ToList();
            foreach (var category in categories)
            {
                List<int> childrenCategories = category.Children.Select(c => c.Id).ToList();
                category.Products = productRepository
                    .FindAll(x => x.HotFlag == true && (x.CategoryId == category.Id || childrenCategories.Contains(x.CategoryId)))
                    .OrderByDescending(x => x.PurchasedCount)
                    .Take(topProduct)
                    .ProjectTo<ProductViewModel>().ToList();
            }
            return categories;
        }

        public void Reorder(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var source = productCategoryRepository.FindById(sourceId);
            var target = productCategoryRepository.FindById(targetId);
            if (source.ParentId != target.ParentId)
            {
                source.ParentId = target.ParentId;
                productCategoryRepository.Update(source);
            }
            // Shift siblings to right
            var sibling = productCategoryRepository.FindAll(c => items.ContainsKey(c.Id));
            foreach (var item in sibling)
            {
                if (item.SortOrder != items[item.Id])
                {
                    item.SortOrder = items[item.Id];
                    productCategoryRepository.Update(item);
                }
            }

        }

        public void Save() => unitOfWork.Commit();

        public void Update(ProductCategoryViewModel productcategoryVm)
        {
            ProductCategory productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productcategoryVm);
            productCategoryRepository.Update(productCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var source = productCategoryRepository.FindById(sourceId);
            source.ParentId = targetId;
            productCategoryRepository.Update(source);

            // get all sibling
            var sibling = productCategoryRepository.FindAll(c => items.ContainsKey(c.Id));
            foreach (var child in sibling)
            {
                if (child.SortOrder != items[child.Id])
                {
                    child.SortOrder = items[child.Id];
                    productCategoryRepository.Update(child);
                }
            }
        }
    }
}