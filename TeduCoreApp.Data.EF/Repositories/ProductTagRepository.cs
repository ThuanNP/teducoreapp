using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;

namespace TeduCoreApp.Data.EF.Repositories
{
    public class ProductTagRepository : EFRepository<ProductTag, string>, IProductTagRepository
    {
        public ProductTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}
