using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;

namespace TeduCoreApp.Application.Implementations
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository, IUnitOfWork unitOfWork)
        {
            _productCategoryRepository = productCategoryRepository;
            _unitOfWork = unitOfWork;
        }

        public ProductCategoryViewModel Add(ProductCategoryViewModel productcategoryVm)
        {
            ProductCategory productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productcategoryVm);
            _productCategoryRepository.Add(productCategory);
            return productcategoryVm;
        }

        public void Delete(int id) => _productCategoryRepository.Remove(id);

        public void Dispose() => GC.SuppressFinalize(this);

        public List<ProductCategoryViewModel> GetAll()
        {
            return _productCategoryRepository.FindAll()
                .OrderBy(x => x.ParentId)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAll(string keyword)
        {
            if (!string.IsNullOrEmpty(keyword))
                return _productCategoryRepository.FindAll(x => x.Name.Contains(keyword) || x.Description.Contains(keyword))
                    .OrderBy(x => x.ParentId)
                    .ProjectTo<ProductCategoryViewModel>().ToList();
            else
                return _productCategoryRepository.FindAll()
                   .OrderBy(x => x.ParentId)
                   .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public List<ProductCategoryViewModel> GetAllByParentId(int parentId)
        {
            return _productCategoryRepository.FindAll(x => x.Status == Status.Active && x.ParentId == parentId)
                .OrderBy(x => x.SortOrder)
                .ProjectTo<ProductCategoryViewModel>().ToList();
        }

        public ProductCategoryViewModel GetById(int id)
        {
            return Mapper.Map<ProductCategory, ProductCategoryViewModel>(_productCategoryRepository.FindById(id));
        }

        public List<ProductCategoryViewModel> GetHomeCategories(int top)
        {
            var query = _productCategoryRepository.FindAll(x => x.HomeFlag == true, c => c.Products)
                  .OrderBy(x => x.HomeOrder)
                  .Take(top).ProjectTo<ProductCategoryViewModel>();

            var categories = query.ToList();
            foreach (var category in categories)
            {
                //category.Products = _productRepository
                //    .FindAll(x => x.HotFlag == true && x.CategoryId == category.Id)
                //    .OrderByDescending(x => x.DateCreated)
                //    .Take(5)
                //    .ProjectTo<ProductViewModel>().ToList();
            }
            return categories;
        }

        public void Reorder(int sourceId, int targetId, string point)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            var target = _productCategoryRepository.FindById(targetId);
            if (source.ParentId != target.ParentId)
            {
                source.ParentId = target.ParentId;
            }
            if (point.ToLower().Equals("top"))
            {
                source.SortOrder = target.SortOrder;
                target.SortOrder++;
            }
            else
            {
                int count = _productCategoryRepository.FindAll(x => x.Status == Status.Active && x.ParentId == source.ParentId).Count();
                source.SortOrder = Math.Min(target.SortOrder + 1, count - 1);
            }
            int minOrder = Math.Min(target.SortOrder, source.SortOrder);
            var children = _productCategoryRepository.FindAll(x => x.ParentId == source.ParentId
                            && x.SortOrder >= minOrder
                            && x.Id != (point.Equals("top") ? targetId : sourceId)).OrderBy(x => x.SortOrder).ToList();
            int maxOrder = Math.Max(target.SortOrder, source.SortOrder);
            foreach (var item in children)
            {
                item.SortOrder = ++maxOrder;
                _productCategoryRepository.Update(item);
            }
            _productCategoryRepository.Update(source);
            _productCategoryRepository.Update(target);

        }

        public void Save() => _unitOfWork.Commit();

        public void Update(ProductCategoryViewModel productcategoryVm)
        {
            ProductCategory productCategory = Mapper.Map<ProductCategoryViewModel, ProductCategory>(productcategoryVm);
            _productCategoryRepository.Update(productCategory);
        }

        public void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items)
        {
            var source = _productCategoryRepository.FindById(sourceId);
            source.ParentId = targetId;
            _productCategoryRepository.Update(source);

            // get all sibling
            var sibling = _productCategoryRepository.FindAll(c => items.ContainsKey(c.Id));
            foreach (var child in sibling)
            {
                child.SortOrder = items[child.Id];
                _productCategoryRepository.Update(child);
            }
        }
    }
}