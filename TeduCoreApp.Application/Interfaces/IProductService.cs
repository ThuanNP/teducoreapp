using System;
using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {      

        List<ProductViewModel> GetAll();

        List<ProductViewModel> GetAll(int? categoryId, string keyword);

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        ProductViewModel GetById(int id);

        ProductViewModel Add(ProductViewModel productViewModel);

        void Update(ProductViewModel productViewModel);

        void Delete(int id);

        void ImportExcel(string filePath, int categoryId);

        void Save();

        // Quantities management

        void AddQuantities(int productId, List<ProductQuantityViewModel> quantityViewModels);

        List<ProductQuantityViewModel> GetQuantities(int productId);

    }
}