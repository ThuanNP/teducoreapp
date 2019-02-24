using AutoMapper;
using AutoMapper.QueryableExtensions;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Blog;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.Utilities.Constants;
using TeduCoreApp.Utilities.Dtos;
using TeduCoreApp.Utilities.Helpers;

namespace TeduCoreApp.Application.Implementations
{
    public class BlogService : BaseService, IBlogService
    {
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IBlogTagRepository _blogTagRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogService(IBlogRepository blogRepository, ITagRepository tagRepository, IBlogTagRepository blogTagRepository, IUnitOfWork unitOfWork)
        {
            _blogRepository = blogRepository;
            _tagRepository = tagRepository;
            _blogTagRepository = blogTagRepository;
            _unitOfWork = unitOfWork;
        }

        public BlogViewModel Add(BlogViewModel blogViewModel)
        {
            var blog = Mapper.Map<BlogViewModel, Blog>(blogViewModel);
            blog.SeoAlias = TextHelper.ToUnsignString(blog.Name);
            if (!string.IsNullOrEmpty(blogViewModel.Tags))
            {
                var tags = blogViewModel.Tags.Split(',');
                foreach (var t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.BlogTag
                        };
                        _tagRepository.Add(tag);
                    }
                }
            }
            _blogRepository.Add(blog);
            return blogViewModel;
        }

        public void Delete(int id)
        {
            _blogRepository.Remove(id);
        }

        public List<BlogViewModel> GetAll()
        {
            return _blogRepository.FindAll(c => c.BlogTags).ProjectTo<BlogViewModel>().ToList();
        }

        public PagedResult<BlogViewModel> GetAllPaging(string keyword, int pageSize, int page)
        {
            var query = _blogRepository.FindAll();
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            int totalRow = query.Count();
            var data = query.OrderByDescending(x => x.DateCreated).Skip((page - 1) * pageSize).Take(pageSize);
            var paginationSet = new PagedResult<BlogViewModel>()
            {
                Results = data.ProjectTo<BlogViewModel>().ToList(),
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize,
            };
            return paginationSet;
        }

        public BlogViewModel GetById(int id)
        {
            var blog = _blogRepository.FindById(id);
            return Mapper.Map<Blog, BlogViewModel>(blog);
        }

        public List<BlogViewModel> GetHotest(int top)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == true);
            return query.OrderByDescending(x => x.DateCreated).Take(top).ProjectTo<BlogViewModel>().ToList();
        }

        public List<BlogViewModel> GetLatest(int top)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active && x.HotFlag == false);
            return query.OrderByDescending(x => x.DateCreated).Take(top).ProjectTo<BlogViewModel>().ToList();
        }

        public List<BlogViewModel> GetList(string keyword)
        {
            //var query = !string.IsNullOrEmpty(keyword) ?
            //    _blogRepository.FindAll(x => x.Name.Contains(keyword)).ProjectTo<BlogViewModel>()
            //   : _blogRepository.FindAll().ProjectTo<BlogViewModel>();
            var query = _blogRepository.FindAll(x => x.Name.Contains(keyword)).ProjectTo<BlogViewModel>();
            return query.ToList();
        }

        public List<string> GetListByName(string name)
        {
            var blogs = _blogRepository.FindAll(x => x.Status == Status.Active && x.Name.Contains(name));
            return blogs.Select(y => y.Name).ToList();
        }

        public List<BlogViewModel> GetListByTag(string tagId, int page, int pageSize, out int totalRow)
        {
            var query = from p in _blogRepository.FindAll()
                        join pt in _blogTagRepository.FindAll()
                        on p.Id equals pt.BlogId
                        where pt.TagId == tagId && p.Status == Status.Active
                        orderby p.DateCreated descending
                        select p;
            totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            return query.ProjectTo<BlogViewModel>().ToList();
        }

        public List<BlogViewModel> GetListPaging(int page, int pageSize, string sort, out int totalRow)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active);
            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<BlogViewModel>().ToList();
        }

        public List<TagViewModel> GetListTag(string searchText)
        {
            var query = _tagRepository.FindAll(x => x.Type == CommonConstants.ProductTag && searchText.Contains(x.Name));
            return query.ProjectTo<TagViewModel>().ToList();
        }

        public List<TagViewModel> GetListTagById(int id)
        {
            var blog = _blogTagRepository.FindAll(x => x.BlogId == id, c => c.Tag);
            return blog.Select(y => y.Tag).ProjectTo<TagViewModel>().ToList();
        }

        public List<BlogViewModel> GetReatedBlogs(int id, int top)
        {
            var blogs = _blogRepository.FindAll(x => x.Status == Status.Active && x.Id != id);
            return blogs.OrderByDescending(x => x.DateCreated).Take(top).ProjectTo<BlogViewModel>().ToList();
        }

        public TagViewModel GetTag(string tagId)
        {
            var tag = _tagRepository.FindSingle(x => x.Id == tagId);
            return Mapper.Map<Tag, TagViewModel>(tag);
        }

        public void IncreaseView(int id)
        {
            var blog = _blogRepository.FindById(id);
            if (blog.ViewCount.HasValue)
            {
                blog.ViewCount++;
            }
            else
            {
                blog.ViewCount = 1;
            }
        }

        public void Save() => _unitOfWork.Commit();

        public List<BlogViewModel> Search(string keyword, int page, int pageSize, string sort, out int totalRow)
        {
            var query = _blogRepository.FindAll(x => x.Status == Status.Active && x.Name.Contains(keyword));
            switch (sort)
            {
                case "popular":
                    query = query.OrderByDescending(x => x.ViewCount);
                    break;

                default:
                    query = query.OrderByDescending(x => x.DateCreated);
                    break;
            }
            totalRow = query.Count();
            return query.Skip((page - 1) * pageSize).Take(pageSize).ProjectTo<BlogViewModel>().ToList();
        }

        public void Update(BlogViewModel blogViewModel)
        {
            var blog = Mapper.Map<BlogViewModel, Blog>(blogViewModel);
            _blogTagRepository.RemoveMultiple(_blogTagRepository.FindAll(x => x.BlogId == blogViewModel.Id).ToList());
            if (!string.IsNullOrEmpty(blogViewModel.Tags))
            {
                string[] tags = blogViewModel.Tags.Split(',');
                foreach (string t in tags)
                {
                    var tagId = TextHelper.ToUnsignString(t);
                    if (!_tagRepository.FindAll(x => x.Id == tagId).Any())
                    {
                        Tag tag = new Tag
                        {
                            Id = tagId,
                            Name = t,
                            Type = CommonConstants.ProductTag
                        };
                        _tagRepository.Add(tag);
                    }
                    BlogTag blogTag = new BlogTag
                    {
                        BlogId = blogViewModel.Id,
                        TagId = tagId
                    };
                    blog.BlogTags.Add(blogTag);
                }
            }
            blog.SeoAlias = TextHelper.ToUnsignString(blog.Name);
            _blogRepository.Update(blog);
        }
    }
}