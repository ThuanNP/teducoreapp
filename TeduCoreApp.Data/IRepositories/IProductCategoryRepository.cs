using System.Collections.Generic;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.infrastructure.Interfaces;

namespace TeduCoreApp.Data.IRepositories
{
    public interface IProductCategoryRepository : IRepository<ProductCategory, int>
    {
        List<ProductCategory> GetByAlias(string alias);
    }
}