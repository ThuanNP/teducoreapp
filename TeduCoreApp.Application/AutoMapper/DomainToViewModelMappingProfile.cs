using AutoMapper;
using TeduCoreApp.Application.ViewModels;
using TeduCoreApp.Application.ViewModels.Blog;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.Entities;

namespace TeduCoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            // Sysytem
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();
            CreateMap<Permission, PermissionViewModel>();
            CreateMap<Function, FunctionViewModel>();
            // Common
            CreateMap<Slide, SlideViewModel>();
            CreateMap<Footer, FooterViewModel>();
            CreateMap<SystemConfig, SystemConfigViewModel>();
            //Product
            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductQuantity, ProductQuantityViewModel>();
            CreateMap<ProductImage, ProductImageViewModel>();
            CreateMap<WholePrice, WholePriceViewModel>();            
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();

            //Content
            CreateMap<Blog, BlogViewModel>();

            //Tag
            CreateMap<ProductTag, ProductTagViewModel>();
            CreateMap<BlogTag, BlogTagViewModel>();           
            CreateMap<Tag, TagViewModel>();
        }
    }
}