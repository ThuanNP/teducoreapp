using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Application.ViewModels.Common
{
    public class ShippingMethodViewModel
    {
        public int Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Period { get; set; }

        public List<BillViewModel> Bills { get; set; }
    }
}
