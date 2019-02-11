using AutoMapper;
using TeduCoreApp.Application.ViewModels;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.Entities;

namespace TeduCoreApp.Application.AutoMapper
{
    public class DomainToViewModelMappingProfile : Profile
    {
        public DomainToViewModelMappingProfile()
        {
            CreateMap<AppUser, AppUserViewModel>();
            CreateMap<AppRole, AppRoleViewModel>();

            CreateMap<Permission, PermissionViewModel>();
            CreateMap<Function, FunctionViewModel>();

            CreateMap<ProductCategory, ProductCategoryViewModel>();
            CreateMap<Product, ProductViewModel>();
            CreateMap<ProductQuantity, ProductQuantityViewModel>();
            CreateMap<ProductImage, ProductImageViewModel>();
            CreateMap<Bill, BillViewModel>();
            CreateMap<BillDetail, BillDetailViewModel>();
            CreateMap<Color, ColorViewModel>();
            CreateMap<Size, SizeViewModel>();

            CreateMap<ProductTag, ProductTagViewModel>();
            CreateMap<Tag, TagViewModel>();
            
           
        }
    }
}