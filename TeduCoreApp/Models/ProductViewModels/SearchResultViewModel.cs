using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Models.ProductViewModels
{
    public class SearchResultViewModel : ProductPagingViewModel
    {
        public string Keyword { get; set; }

        public ProductCategoryViewModel Category { get; set; }
    }
}