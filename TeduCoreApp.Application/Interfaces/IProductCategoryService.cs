using System;
using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IProductCategoryService : IDisposable
    {
        ProductCategoryViewModel Add(ProductCategoryViewModel productcategoryVm);

        void Update(ProductCategoryViewModel productcategoryVm);

        void Delete(int id);

        List<ProductCategoryViewModel> GetAll();

        List<ProductCategoryViewModel> GetAll(string keyword);

        List<ProductCategoryViewModel> GetAllByParentId(int parentId);

        ProductCategoryViewModel GetById(int id);

        void UpdateParentId(int sourceId, int targetId, Dictionary<int, int> items);

        List<ProductCategoryViewModel> GetHomeCategories(int topCategory, int topProduct);

        void Reorder(int sourceId, int targetId, Dictionary<int, int> items);

        void Save();
    }
}