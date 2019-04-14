using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;

namespace TeduCoreApp.Data.EF.Repositories
{
    public class ShippingMethodRepository : EFRepository<ShippingMethod, int>, IShippingMethodRepository
    {
        public ShippingMethodRepository(AppDbContext context) : base(context)
        {
        }
    }
}
