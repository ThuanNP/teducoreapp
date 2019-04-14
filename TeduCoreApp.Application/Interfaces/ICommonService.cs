using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.ViewModels.Common;
using TeduCoreApp.Application.ViewModels.Product;

namespace TeduCoreApp.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();

        Task<List<SlideViewModel>> GetSlidesAsync(string groupAlias, int top = 5);

        List<SlideViewModel> GetSlides(string groupAlias, int top = 5);

        SystemConfigViewModel GetSystemConfig(string code);

        List<ColorViewModel> GetColors();

        List<SizeViewModel> GetSizes();

        ColorViewModel GetColor(int id);

        SizeViewModel GetSize(int id);

        List<ShippingMethodViewModel> GetShippingMethods();
        ShippingMethodViewModel GetShippingMethod(int id);
    }
}