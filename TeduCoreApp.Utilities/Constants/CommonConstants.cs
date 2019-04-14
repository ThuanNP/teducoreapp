namespace TeduCoreApp.Utilities.Constants
{
    public class CommonConstants
    {
        public const string DefaultFooterId = "DefaultFooterId";
        public const string ProductTag = "Product";
        public const string BlogTag = "Blog";
        public const string CartSession = "CartSession";

        public sealed class SlideGroupAlias
        {
            public const string Top = "top";
            public const string Brand = "brand";
        }

        public sealed class UserClaims
        {
            public const string Roles = "Roles";
        }

        public sealed class AppRoles
        {
            public const string AdminRole = "Admin";
            public const string UserRole = "User";
        }

        public sealed class ProductSortType
        {
            public const string Latest = "Latest";
            public const string PriceAsc = "Price Ascent";
            public const string PriceDesc = "Price Descent";
            public const string Name = "Name";
        }

        public sealed class BodyCssClass
        {
            public const string HomeIndex = "cms-index-index cms-home-page";
            public const string HomeAbout = "about_us_page";
            public const string ProductCatalog = "shop_grid_full_width_page";
            public const string ProductDetail = "product-page";
            public const string CartIndex = "shopping_cart_page";
            public const string Checkout = "checkout_page";
        }
    }
}