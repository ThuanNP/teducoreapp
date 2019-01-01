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
using TeduCoreApp.Utilities.Constants;
using TeduCoreApp.Utilities.Dtos;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IProductTagRepository _productTagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IProductRepository productRepository, ITagRepository tagRepository, IProductTagRepository productTagRepository, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _tagRepository = tagRepository;
            _productTagRepository = productTagRepository;
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
                    if (!_tagRepository.FindAll(tag=>tag.Id == tagId).Any())
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

        public void Delete(int id)
        {
            _productRepository.Remove(id);
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(p => p.ProductCategory)
                .ProjectTo<ProductViewModel>().ToList();
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
            product.SeoAlias = TextHelper.ToUnsignString(productViewModel.Name);
            _productRepository.Update(product);
        }
    }
}
