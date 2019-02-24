using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using static TeduCoreApp.Utilities.Constants.CommonConstants;

namespace TeduCoreApp.Controllers.Components
{
    public class BrandViewComponent : ViewComponent
    {
        private readonly ICommonService commonService;

        public BrandViewComponent(ICommonService commonService)
        {
            this.commonService = commonService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(commonService.GetSlides(SlideGroupAlias.Brand, 20));
        }
    }
}
