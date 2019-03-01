using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Models.ProductViewModels
{
    public abstract class ProductPagingViewModel
    {
        public PagedResult<ProductViewModel> Data { get; set; }

        public string SortType { get; set; }
        public int? PageSize { get; set; }

        public List<SelectListItem> SortTypes { get; } = new List<SelectListItem>
        {
            new SelectListItem{Value="latest", Text="Latest"},
            new SelectListItem{Value="price", Text="Price"},
            new SelectListItem{Value="name", Text="Name"}
        };

        public List<SelectListItem> PageSizes { get; } = new List<SelectListItem>
        {
            new SelectListItem{Value="8", Text="8"},
            new SelectListItem{Value="12", Text="12"},
            new SelectListItem{Value="24", Text="24"},
            new SelectListItem{Value="48", Text="48"}
        };
    }
}