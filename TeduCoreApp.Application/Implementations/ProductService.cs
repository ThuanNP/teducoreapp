using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;

namespace TeduCoreApp.Application.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public void Dispose() => GC.SuppressFinalize(this);

        public List<ProductViewModel> GetAll()
        {
            return _productRepository.FindAll(p => p.ProductCategory)
                .ProjectTo<ProductViewModel>().ToList();
        }
    }
}
