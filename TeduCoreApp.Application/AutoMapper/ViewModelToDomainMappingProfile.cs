using AutoMapper;
using System;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.Entities;

namespace TeduCoreApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId,
                c.HomeOrder, c.Image, c.HomeFlag, c.SeoPageTitle, c.SeoAlias, c.Seokeywords,
                c.SeoDecription, c.Status, c.SortOrder));
            CreateMap<ProductViewModel, Product>()
                .ConstructUsing(c => new Product(c.Name, c.CategoryId, c.Image, c.Price, c.OriginalPrice,
                c.PromotionPrice, c.Description, c.Content, c.HomeFlag, c.HotFlag, c.Tags, c.Unit, c.Status,
                c.SeoPageTitle, c.SeoAlias, c.SeoKeywords, c.SeoDescription));
            CreateMap<AppUserViewModel, AppUser>()
                .ConstructUsing(c => new AppUser(c.Id.GetValueOrDefault(Guid.Empty),
                c.FullName, c.Balance, c.Avatar, c.DateCreated, c.Status));
            CreateMap<AppRoleViewModel, AppRole>()
                .ConstructUsing(c => new AppRole(c.Name, c.Description));
            CreateMap<PermissionViewModel, Permission>()
                .ConstructUsing(c => new Permission(c.RoleId, c.FunctionId, c.CanCreate, c.CanRead, c.CanUpdate, c.CanDelete));
            CreateMap<BillViewModel, Bill>()
                .ConstructUsing(c => new Bill(c.Id, c.CustomerId, c.CustomerName, c.CustomerAddress, c.CustomerMobile, c.CustomerEmail, c.CustomerMessage, c.BillStatus, c.PaymentMethod, c.Status));
            CreateMap<BillDetailViewModel, BillDetail>()
                .ConstructUsing(c => new BillDetail(c.Id, c.BillId, c.ProductId, c.Quantity, c.Price, c.ColorId, c.SizeId));
        }
    }
}