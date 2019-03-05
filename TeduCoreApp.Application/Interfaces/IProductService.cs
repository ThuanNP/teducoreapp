using System;
using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {

        List<ProductViewModel> GetAll();

        List<ProductViewModel> GetAll(int? categoryId, string keyword);

        List<ProductViewModel> GetTopRelated(int id, int top);

        List<ProductViewModel> GetTopSpecialOffers(int top);

        List<ProductViewModel> GetTopBestSellers(int top);

        List<ProductViewModel> GetTopNew(int top);

        List<ProductViewModel> GetTopViewCount(int top);

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize, string sortBy);

        PagedResult<ProductViewModel> GetAllTaging(string tagId, int page, int pageSize, string sortBy);

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

        // Tags
        List<TagViewModel> GetTags(int productId);

        bool CheckAvailability(int productId, int sizeId, int colorId);

    }
}