using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Extensions;
using TeduCoreApp.Models;
using TeduCoreApp.Utilities.Constants;
using static TeduCoreApp.Utilities.Constants.CommonConstants;

namespace TeduCoreApp.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService productService;
        private readonly ICommonService commonService;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public CartController(IProductService productService, ICommonService commonService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.productService = productService;
            this.commonService = commonService;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        [HttpGet, Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            ViewData["BodyClass"] = BodyCssClass.CartIndex;
            return View();
        }

        [HttpGet, Route("checkout.html", Name = "Checkout")]
        public IActionResult Checkout()
        {
            ViewData["BodyClass"] = BodyCssClass.Checkout;
            var cartItems = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession) ?? new List<ShoppingCartViewModel>();

            var model = new CheckoutViewModel
            {
                ShoppingCarts = cartItems,
                ShippingMethods = commonService.GetShippingMethods(),
                ShippingMethod = commonService.GetShippingMethod(1)
            };
            if (signInManager.IsSignedIn(User))
            {
                var id = userManager.GetUserId(User);
                var user = userManager.GetUserAsync(User).Result;
                model.CustomerName = user.FullName;
                model.CustomerEmail = user.Email;
                model.CustomerMobile = user.PhoneNumber;
                model.CustomerAddress = user.Address;
            }
            return View(model);
        }

        #region AJAX Request
        /// <summary>
        /// Get shopping item list
        /// </summary>
        /// <returns></returns>
        public IActionResult GetCart()
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(key: CartSession) ?? new List<ShoppingCartViewModel>();
            return new OkObjectResult(session);
        }

        /// <summary>
        /// Remove all shopping items
        /// </summary>
        /// <returns></returns>
        public IActionResult ClearCart()
        {
            HttpContext.Session.Remove(CommonConstants.CartSession);
            return new OkObjectResult("OK");
        }

        /// <summary>
        /// Add products to cart
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <param name="color"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, int color = 1, int size = 1)
        {
            bool hasChanged = false;
            //Get product detail
            var product = productService.GetById(productId);
            //Get session with item list from cart
            var shoppingCarts = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession) ?? new List<ShoppingCartViewModel>();

            //Check exist with item product id
            if (shoppingCarts.Any(x => x.Product.Id == productId))
            {
                foreach (var item in shoppingCarts)
                {
                    //Update quantity for product if match product id
                    if (item.Product.Id == productId)
                    {
                        item.Quantity += quantity;
                        item.Color = commonService.GetColor(id: color);
                        item.Size = commonService.GetSize(id: size);
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }
            }
            else
            {
                shoppingCarts.Add(new ShoppingCartViewModel
                {
                    Product = product,
                    Quantity = quantity,
                    Color = commonService.GetColor(id: color) ?? commonService.GetColors().FirstOrDefault(),
                    Size = commonService.GetSize(id: size) ?? commonService.GetSizes().FirstOrDefault(),
                    Price = product.PromotionPrice ?? product.Price
                });
                hasChanged = true;
            }
            //Update back to cart
            if (hasChanged)
            {
                HttpContext.Session.Set(CommonConstants.CartSession, shoppingCarts);
                return new OkObjectResult(product.Name);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Remove a product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public IActionResult RemoveFromCart(int productId)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        session.Remove(item);
                        hasChanged = true;
                        break;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        /// <summary>
        /// Update product quantity
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="quantity"></param>
        /// <returns></returns>
        public IActionResult UpdateCart(int productId, int quantity, int color, int size)
        {
            var session = HttpContext.Session.Get<List<ShoppingCartViewModel>>(CommonConstants.CartSession);
            if (session != null)
            {
                bool hasChanged = false;
                foreach (var item in session)
                {
                    if (item.Product.Id == productId)
                    {
                        var product = productService.GetById(productId);
                        item.Product = product;
                        item.Quantity = quantity;
                        item.Color = commonService.GetColor(color);
                        item.Size = commonService.GetSize(size);
                        item.Price = product.PromotionPrice ?? product.Price;
                        hasChanged = true;
                    }
                }
                if (hasChanged)
                {
                    HttpContext.Session.Set(CommonConstants.CartSession, session);
                }
                return new OkObjectResult(productId);
            }
            return new EmptyResult();
        }

        [HttpGet]
        public IActionResult GetColors()
        {
            var colors = commonService.GetColors();
            return new OkObjectResult(colors);
        }

        [HttpGet]
        public IActionResult GetSizes()
        {
            var sizes = commonService.GetSizes();
            return new OkObjectResult(sizes);
        }
        #endregion
    }
}