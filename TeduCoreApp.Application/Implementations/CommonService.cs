﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;
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
        private readonly IColorRepository colorRepository;
        private readonly ISizeRepository sizeRepository;
        private readonly IShippingMethodRepository shippingMethodRepository;
        private readonly IUnitOfWork unitOfWork;

        public CommonService(IFooterRepository footerRepository, ISystemConfigRepository systemConfigRepository, ISlideRepository slideRepository, IColorRepository colorRepository, ISizeRepository sizeRepository, IShippingMethodRepository shippingMethodRepository, IUnitOfWork unitOfWork)
        {
            this.footerRepository = footerRepository;
            this.systemConfigRepository = systemConfigRepository;
            this.slideRepository = slideRepository;
            this.colorRepository = colorRepository;
            this.sizeRepository = sizeRepository;
            this.shippingMethodRepository = shippingMethodRepository;
            this.unitOfWork = unitOfWork;
        }

        public ColorViewModel GetColor(int id)
        {
            var color = colorRepository.FindById(id);
            return Mapper.Map<Color, ColorViewModel>(color);
        }

        public List<ColorViewModel> GetColors()
        {
            return colorRepository.FindAll().ProjectTo<ColorViewModel>().ToList();
        }

        public FooterViewModel GetFooter()
        {
            Footer footer = footerRepository.FindSingle(x => x.Id == CommonConstants.DefaultFooterId);
            return Mapper.Map<Footer, FooterViewModel>(footer);
        }

        public ShippingMethodViewModel GetShippingMethod(int id)
        {
            var method = shippingMethodRepository.FindById(id);
            return Mapper.Map<ShippingMethod, ShippingMethodViewModel>(method);
        }

        public List<ShippingMethodViewModel> GetShippingMethods()
        {
            return shippingMethodRepository.FindAll().ProjectTo<ShippingMethodViewModel>().OrderBy(x => x.Price).ToList();
        }

        public SizeViewModel GetSize(int id)
        {
            var size = sizeRepository.FindById(id);
            return Mapper.Map<Size, SizeViewModel>(size);
        }

        public List<SizeViewModel> GetSizes()
        {
            return sizeRepository.FindAll().ProjectTo<SizeViewModel>().ToList();
        }

        public List<SlideViewModel> GetSlides(string groupAlias, int top = 5)
        {
            IQueryable<Slide> slides = slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias);
            return slides.OrderBy(x => x.DisplayOrder).Take(top).ProjectTo<SlideViewModel>().ToList();
        }

        public async Task<List<SlideViewModel>> GetSlidesAsync(string groupAlias, int top = 5)
        {
            IQueryable<Slide> slides = slideRepository.FindAll(x => x.Status && x.GroupAlias == groupAlias);
            return await slides.OrderBy(x => x.DisplayOrder).Take(top).ProjectTo<SlideViewModel>().ToListAsync();
        }

        public SystemConfigViewModel GetSystemConfig(string code)
        {
            SystemConfig config = systemConfigRepository.FindSingle(x => x.Id == code);
            return Mapper.Map<SystemConfig, SystemConfigViewModel>(config);
        }
    }
}
