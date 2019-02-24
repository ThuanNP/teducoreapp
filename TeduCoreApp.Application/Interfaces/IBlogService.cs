using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Blog;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IBlogService : IBaseService
    {
        BlogViewModel Add(BlogViewModel blogViewModel);

        void Update(BlogViewModel blogViewModel);

        void Delete(int id);

        List<BlogViewModel> GetAll();

        PagedResult<BlogViewModel> GetAllPaging(string keyword, int pageSize, int page);

        List<BlogViewModel> GetLatest(int top);

        List<BlogViewModel> GetHotest(int top);

        List<BlogViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow);

        List<BlogViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow);

        List<BlogViewModel> GetList(string keyword);

        List<BlogViewModel> GetReatedBlogs(int id, int top);

        List<string> GetListByName(string name);

        BlogViewModel GetById(int id);

        void Save();

        List<TagViewModel> GetListTagById(int id);

        TagViewModel GetTag(string tagId);

        void IncreaseView(int id);

        List<BlogViewModel> GetListByTag(string tagId, int page, int pageSize, out int totalRow);

        List<TagViewModel> GetListTag(string searchText);
    }
}