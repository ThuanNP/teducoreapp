using AutoMapper;
using System;
using TeduCoreApp.Application.ViewModels.Blog;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.Entities;

namespace TeduCoreApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            //System
            CreateMap<AppUserViewModel, AppUser>()
                .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty), c.FullName, c.Email, c.PhoneNumber, c.Avatar, c.Status));
            CreateMap<AppRoleViewModel, AppRole>()
                .ConstructUsing(c => new AppRole(c.Name, c.Description));
            CreateMap<PermissionViewModel, Permission>()
                .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));
            //Product
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag, c.SeoPageTitle, c.SeoAlias, c.Seokeywords,
                c.SeoDecription, c.Status, c.SortOrder));
            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice, c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status, c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));
            CreateMap<ProductQuantityViewModel, ProductQuantity>()
                .ConstructUsing(c => new ProductQuantity(c.ProductId, c.SizeId, c.ColorId, c.Quantity));
            CreateMap<BillViewModel, Bill>()
                .ConstructUsing(c => new Bill(c.CustomerId, c.CustomerName, c.CustomerAddress, c.CustomerMobile, c.CustomerEmail, c.CustomerMessage, c.BillStatus, c.PaymentMethod, c.ShippingMethod, c.Status));
            CreateMap<BillDetailViewModel, BillDetail>()
                .ConstructUsing(c => new BillDetail(c.BillId, c.ProductId, c.Quantity, c.Price, c.ColorId, c.SizeId));
            //Content
            CreateMap<BlogViewModel, Blog>()
                .ConstructUsing(c => new Blog(c.Name, c.Description, c.Content, c.Image,
                c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription, c.Status));
            //Common
            CreateMap<ColorViewModel, Color>()
               .ConstructUsing(c => new Color(c.Name, c.Code));
            CreateMap<SizeViewModel, Size>()
              .ConstructUsing(c => new Size(c.Name));
        }
    }
}