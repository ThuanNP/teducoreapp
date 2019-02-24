using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;

namespace TeduCoreApp.Application.Implementations
{
    public class TagService : BaseService, ITagService
    {
        private readonly ITagRepository tagRepository;
        private readonly IProductTagRepository productTagRepository;

        public TagService(ITagRepository tagRepository, IProductTagRepository productTagRepository)
        {
            this.tagRepository = tagRepository;
            this.productTagRepository = productTagRepository;
        }

        public List<TagViewModel> GetAll()
        {
            return tagRepository.FindAll().ProjectTo<TagViewModel>().ToList();
        }

        public TagViewModel GetById(string id)
        {
            return Mapper.Map<Tag, TagViewModel>(tagRepository.FindById(id));

        }
    }
}
