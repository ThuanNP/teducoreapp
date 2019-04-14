using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Utilities.Constants;
using ShippingMethod = TeduCoreApp.Data.Entities.ShippingMethod;

namespace TeduCoreApp.Data.EF
{
    public class DbIntinitializer
    {
        private readonly AppDbContext context;
        private UserManager<AppUser> userManager;
        private RoleManager<AppRole> roleManager;

        public DbIntinitializer(AppDbContext context, UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task Seed()
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new AppRole()
                {
                    Name = "Admin",
                    NormalizedName = "Admin",
                    Description = "Top manager"
                });
                await roleManager.CreateAsync(new AppRole()
                {
                    Name = "Staff",
                    NormalizedName = "Staff",
                    Description = "Staff"
                });
                await roleManager.CreateAsync(new AppRole()
                {
                    Name = "Customer",
                    NormalizedName = "Customer",
                    Description = "Customer"
                });
            }

            if (!userManager.Users.Any())
            {
                await userManager.CreateAsync(new AppUser()
                {
                    UserName = "admin",
                    FullName = "Administrator",
                    Email = "admin@gmail.com",
                    Balance = 0,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    Status = Status.Active
                }, "123654$");
                var user = await userManager.FindByNameAsync("admin");
                await userManager.AddToRoleAsync(user, "Admin");
            }

            if (context.Functions.Count() == 0)
            {
                List<Function> functions = new List<Function>()
                {
                    new Function() {Id = "SYSTEM", Name = "System",ParentId = null,SortOrder = 1,Status = Status.Active,URL = "/",IconCss = "fa-desktop"  },
                    new Function() {Id = "ROLE", Name = "Role",ParentId = "SYSTEM",SortOrder = 1,Status = Status.Active,URL = "/admin/role/index",IconCss = "fa-home"  },
                    new Function() {Id = "FUNCTION", Name = "Function",ParentId = "SYSTEM",SortOrder = 2,Status = Status.Active,URL = "/admin/function/index",IconCss = "fa-home"  },
                    new Function() {Id = "USER", Name = "User",ParentId = "SYSTEM",SortOrder =3,Status = Status.Active,URL = "/admin/user/index",IconCss = "fa-home"  },
                    new Function() {Id = "ACTIVITY", Name = "Activity",ParentId = "SYSTEM",SortOrder = 4,Status = Status.Active,URL = "/admin/activity/index",IconCss = "fa-home"  },
                    new Function() {Id = "ERROR", Name = "Error",ParentId = "SYSTEM",SortOrder = 5,Status = Status.Active,URL = "/admin/error/index",IconCss = "fa-home"  },
                    new Function() {Id = "SETTING", Name = "Configuration",ParentId = "SYSTEM",SortOrder = 6,Status = Status.Active,URL = "/admin/setting/index",IconCss = "fa-home"  },
                    new Function() {Id = "PRODUCT",Name = "Product Management",ParentId = null,SortOrder = 2,Status = Status.Active,URL = "/",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "PRODUCT_CATEGORY",Name = "Category",ParentId = "PRODUCT",SortOrder =1,Status = Status.Active,URL = "/admin/productcategory/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "PRODUCT_LIST",Name = "Product",ParentId = "PRODUCT",SortOrder = 2,Status = Status.Active,URL = "/admin/product/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "BILL",Name = "Bill",ParentId = "PRODUCT",SortOrder = 3,Status = Status.Active,URL = "/admin/bill/index",IconCss = "fa-chevron-down"  },
                    new Function() {Id = "CONTENT",Name = "Content",ParentId = null,SortOrder = 3,Status = Status.Active,URL = "/",IconCss = "fa-table"  },
                    new Function() {Id = "BLOG",Name = "Blog",ParentId = "CONTENT",SortOrder = 1,Status = Status.Active,URL = "/admin/blog/index",IconCss = "fa-table"  },
                    new Function() {Id = "UTILITY",Name = "Utilities",ParentId = null,SortOrder = 4,Status = Status.Active,URL = "/",IconCss = "fa-clone"  },
                    new Function() {Id = "FOOTER",Name = "Footer",ParentId = "UTILITY",SortOrder = 1,Status = Status.Active,URL = "/admin/footer/index",IconCss = "fa-clone"  },
                    new Function() {Id = "FEEDBACK",Name = "Feedback",ParentId = "UTILITY",SortOrder = 2,Status = Status.Active,URL = "/admin/feedback/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ANNOUNCEMENT",Name = "Announcement",ParentId = "UTILITY",SortOrder = 3,Status = Status.Active,URL = "/admin/announcement/index",IconCss = "fa-clone"  },
                    new Function() {Id = "CONTACT",Name = "Contact",ParentId = "UTILITY",SortOrder = 4,Status = Status.Active,URL = "/admin/contact/index",IconCss = "fa-clone"  },
                    new Function() {Id = "SLIDE",Name = "Slide",ParentId = "UTILITY",SortOrder = 5,Status = Status.Active,URL = "/admin/slide/index",IconCss = "fa-clone"  },
                    new Function() {Id = "ADVERTISMENT",Name = "Advertisment",ParentId = "UTILITY",SortOrder = 6,Status = Status.Active,URL = "/admin/advertistment/index",IconCss = "fa-clone"  },

                    new Function() {Id = "REPORT",Name = "Report",ParentId = null,SortOrder = 5,Status = Status.Active,URL = "/",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "REVENUES",Name = "Revenue report",ParentId = "REPORT",SortOrder = 1,Status = Status.Active,URL = "/admin/report/revenues",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "ACCESS",Name = "Visitor Report",ParentId = "REPORT",SortOrder = 2,Status = Status.Active,URL = "/admin/report/visitor",IconCss = "fa-bar-chart-o"  },
                    new Function() {Id = "READER",Name = "Reader Report",ParentId = "REPORT",SortOrder = 3,Status = Status.Active,URL = "/admin/report/reader",IconCss = "fa-bar-chart-o"  },
                };
                context.Functions.AddRange(functions);
            }

            if (context.Footers.Count(x => x.Id == CommonConstants.DefaultFooterId) == 0)
            {
                string content = "Footer";
                context.Footers.Add(new Footer()
                {
                    Id = CommonConstants.DefaultFooterId,
                    Content = content
                });
            }

            if (context.Colors.Count() == 0)
            {
                context.Colors.AddRange(new List<Color>()
                {
                    new Color() {Name="Black", Code="#000000" },
                    new Color() {Name="White", Code="#FFFFFF"},
                    new Color() {Name="Red", Code="#ff0000" },
                    new Color() {Name="Blue", Code="#1000ff" },
                });
            }

            if (context.AdvertisementPages.Count() == 0)
            {
                List<AdvertisementPage> pages = new List<AdvertisementPage>()
                {
                    new AdvertisementPage() {Id="home", Name="Home", AdvertisementPositions = new List<AdvertisementPosition>(){
                        new AdvertisementPosition(){Id="home-left",Name="Left"}
                    } },
                    new AdvertisementPage() {Id="product-cate", Name="Product category" ,
                        AdvertisementPositions = new List<AdvertisementPosition>(){
                        new AdvertisementPosition(){Id="product-cate-left",Name="Left"}
                    }},
                    new AdvertisementPage() {Id="product-detail", Name="Product detail",
                        AdvertisementPositions = new List<AdvertisementPosition>(){
                        new AdvertisementPosition(){Id="product-detail-left",Name="Left"}
                    }},

                };
                context.AdvertisementPages.AddRange(pages);
            }

            if (context.Slides.Count() == 0)
            {
                List<Slide> slides = new List<Slide>()
                {
                    new Slide() {Name="Slide 1",Image="/client-side/images/slider/slide-1.jpg",Url="#",DisplayOrder = 0,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/slider/slide-2.jpg",Url="#",DisplayOrder = 1,GroupAlias = "top",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/slider/slide-3.jpg",Url="#",DisplayOrder = 2,GroupAlias = "top",Status = true },

                    new Slide() {Name="Slide 1",Image="/client-side/images/brand1.png",Url="#",DisplayOrder = 1,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 2",Image="/client-side/images/brand2.png",Url="#",DisplayOrder = 2,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 3",Image="/client-side/images/brand3.png",Url="#",DisplayOrder = 3,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 4",Image="/client-side/images/brand4.png",Url="#",DisplayOrder = 4,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 5",Image="/client-side/images/brand5.png",Url="#",DisplayOrder = 5,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 6",Image="/client-side/images/brand6.png",Url="#",DisplayOrder = 6,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 7",Image="/client-side/images/brand7.png",Url="#",DisplayOrder = 7,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 8",Image="/client-side/images/brand8.png",Url="#",DisplayOrder = 8,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 9",Image="/client-side/images/brand9.png",Url="#",DisplayOrder = 9,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 10",Image="/client-side/images/brand10.png",Url="#",DisplayOrder = 10,GroupAlias = "brand",Status = true },
                    new Slide() {Name="Slide 11",Image="/client-side/images/brand11.png",Url="#",DisplayOrder = 11,GroupAlias = "brand",Status = true },
                };
                context.Slides.AddRange(slides);
            }

            if (context.Sizes.Count() == 0)
            {
                List<Size> listSize = new List<Size>()
                {
                    new Size() { Name="XXL" },
                    new Size() { Name="XL"},
                    new Size() { Name="L" },
                    new Size() { Name="M" },
                    new Size() { Name="S" },
                    new Size() { Name="XS" }
                };
                context.Sizes.AddRange(listSize);
            }

            if (context.ProductCategories.Count() == 0)
            {
                List<ProductCategory> listProductCategory = new List<ProductCategory>()
                {
                    new ProductCategory { Name="Men shirt",SeoAlias="men-shirt",ParentId = null,Status=Status.Active,SortOrder=1, DateCreated = DateTime.Now,
                        Products = new List<Product>()
                        {
                            new Product {Name = "Product 1",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-1",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 2",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-2",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 3",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-3",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 4",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-4",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 5",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-5",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                        }
                    },
                    new ProductCategory { Name="Women shirt",SeoAlias="women-shirt",ParentId = null,Status=Status.Active ,SortOrder=2, DateCreated = DateTime.Now,
                        Products = new List<Product>()
                        {
                            new Product {Name = "Product 6",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-6",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 7",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-7",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 8",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-8",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 9",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-9",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 10",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-10",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                        }},
                    new ProductCategory { Name="Men shoes",SeoAlias="men-shoes",ParentId = null,Status=Status.Active ,SortOrder=3, DateCreated = DateTime.Now,
                        Products = new List<Product>()
                        {
                            new Product {Name = "Product 11",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-11",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 12",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-12",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 13",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-13",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 14",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-14",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 15",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-15",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                        }},
                    new ProductCategory { Name="Woment shoes",SeoAlias="women-shoes",ParentId = null,Status=Status.Active,SortOrder=4, DateCreated = DateTime.Now,
                        Products = new List<Product>()
                        {
                            new Product {Name = "Product 16",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-16",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 17",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-17",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 18",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-18",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 19",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-19",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                            new Product {Name = "Product 20",Image="/client-side/images/products/product-1.jpg",SeoAlias = "san-pham-20",Price = 1000,Status = Status.Active,OriginalPrice = 1000, DateCreated = DateTime.Now},
                        }}
                };
                context.ProductCategories.AddRange(listProductCategory);
            }

            if (!context.SystemConfigs.Any(x => x.Id == "HomeTitle"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeTitle",
                    Name = "Home's title",
                    Value1 = "Tedu Shop home",
                    Status = Status.Active
                });
            }

            if (!context.SystemConfigs.Any(x => x.Id == "HomeMetaKeyword"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeMetaKeyword",
                    Name = "Home Keyword",
                    Value1 = "shopping, sales",
                    Status = Status.Active
                });
            }

            if (!context.SystemConfigs.Any(x => x.Id == "HomeMetaDescription"))
            {
                context.SystemConfigs.Add(new SystemConfig()
                {
                    Id = "HomeMetaDescription",
                    Name = "Home Description",
                    Value1 = "Home tedu",
                    Status = Status.Active
                });
            }

            if (context.ShippingMethods.Count() == 0)
            {
                context.ShippingMethods.AddRange(new List<ShippingMethod> {
                    new ShippingMethod { Name = "Standard", Period = 7, Price = 0 },
                    new ShippingMethod { Name = "Express", Period = 1, Price = 20000 }
                });
            }

            await context.SaveChangesAsync();
        }
    }
}