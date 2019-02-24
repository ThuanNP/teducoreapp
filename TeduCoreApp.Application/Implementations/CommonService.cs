using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.Utilities.Constants;

namespace TeduCoreApp.Application.Implementations
{
    public class CommonService : ICommonService
    {
        private readonly IFooterRepository footerRepository;
        private readonly ISystemConfigRepository systemConfigRepository;
        private readonly ISlideRepository slideRepository;
        private readonly IUnitOfWork unitOfWork;

        public CommonService(IFooterRepository footerRepository, ISystemConfigRepository systemConfigRepository, ISlideRepository slideRepository, IUnitOfWork unitOfWork)
        {
            this.footerRepository = footerRepository;
            this.systemConfigRepository = systemConfigRepository;
            this.slideRepository = slideRepository;
            this.unitOfWork = unitOfWork;
        }

        public FooterViewModel GetFooter()
        {
            Footer footer = footerRepository.FindSingle(x => x.Id == CommonConstants.DefaultFooterId);
            return Mapper.Map<Footer, FooterViewModel>(footer);
        }

        public List<SlideViewModel> GetSlides(string groupAlias, int top =5)
        {
            IQueryable<Slide> slides = slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias);
            return slides.OrderBy(x=>x.DisplayOrder).Take(top).ProjectTo<SlideViewModel>().ToList();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            SystemConfig config = systemConfigRepository.FindSingle(x => x.Id == code);
            return Mapper.Map<SystemConfig, SystemConfigViewModel>(config);
        }
    }
}
