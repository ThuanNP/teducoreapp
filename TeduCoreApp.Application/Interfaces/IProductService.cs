using System;
using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IProductService : IDisposable
    {      

        List<ProductViewModel> GetAll();

        PagedResult<ProductViewModel> GetAllPaging(int? categoryId, string keyword, int page, int pageSize);

        ProductViewModel GetById(int id);

        ProductViewModel Add(ProductViewModel productViewModel);

        void Update(ProductViewModel productViewModel);

        void Delete(int id);

        void Save();
    }
}