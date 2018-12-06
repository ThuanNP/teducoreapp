using AutoMapper;
using TeduCoreApp.Application.ViewModels;
using TeduCoreApp.Data.Entities;

namespace TeduCoreApp.Application.AutoMapper
{
    public class ViewModelToDomainMappingProfile : Profile
    {
        public ViewModelToDomainMappingProfile()
        {
            CreateMap<ProductCategoryViewModel, ProductCategory>()
                .ConstructUsing(c => new ProductCategory(c.Name, c.Description, c.ParentId, c.HomeOrder, c.Image, c.HomeFlag,
                c.SeoPageTitle, c.SeoAlias, c.Seokeywords, c.SeoDecription, c.Status, c.SortOrder));
        }
    }
}