using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;

namespace TeduCoreApp.Data.EF.Repositories
{
    public class BlogTagRepository : EFRepository<BlogTag, string>, IBlogTagRepository
    {
        public BlogTagRepository(AppDbContext context) : base(context)
        {
        }
    }
}