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
        List<ProductQuantityViewModel> GetQuantities(int productId);

        void AddQuantities(int productId, List<ProductQuantityViewModel> quantityViewModels);

        // Images management
     
        List<ProductImageViewModel> GetImages(int productId);

        void AddImages(int productId, string[] images);

        // whole price management

        List<WholePriceViewModel> GetWholePrices(int productId);

        void AddWholePrice(int productId, List<WholePriceViewModel> wholePrices);

    }
}