using System;
using System.Collections.Generic;
using System.Linq;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Utilities.Extensions;

namespace TeduCoreApp.Models
{
    public class CheckoutViewModel : BillViewModel
    {
        public List<ShoppingCartViewModel> ShoppingCarts { get; set; }

        public List<ShippingMethodViewModel> ShippingMethods { get; set; }

        public List<EnumModel> PaymentMethods
        {
            get
            {
                return ((PaymentMethod[])Enum.GetValues(typeof(PaymentMethod))).Select(c => new EnumModel
                {
                    Value = (int)c,
                    Name = c.GetDescription()
                }).ToList();
            }
        }

      

    }
}