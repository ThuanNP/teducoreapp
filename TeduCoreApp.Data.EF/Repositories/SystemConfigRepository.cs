using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;

namespace TeduCoreApp.Data.EF.Repositories
{
    public class SystemConfigRepository : EFRepository<SystemConfig, string>, ISystemConfigRepository
    {
        public SystemConfigRepository(AppDbContext context) : base(context)
        {
        }
    }
}