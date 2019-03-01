﻿using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Models.ProductViewModels
{
    public class CatalogViewModel : ProductPagingViewModel
    {
        public ProductCategoryViewModel Category { get; set; }
    }
}