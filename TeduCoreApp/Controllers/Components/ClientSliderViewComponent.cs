using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using static TeduCoreApp.Utilities.Constants.CommonConstants;

namespace TeduCoreApp.Controllers.Components
{
    public class ClientSliderViewComponent : ViewComponent
    {
        private readonly ICommonService commonService;

        public ClientSliderViewComponent(ICommonService commonService)
        {
            this.commonService = commonService;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(commonService.GetSlides(SlideGroupAlias.Brand, 20));
        }
    }
}
