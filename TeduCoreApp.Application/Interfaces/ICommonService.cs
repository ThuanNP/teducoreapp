using System.Collections.Generic;
using TeduCoreApp.Application.ViewModels.Common;

namespace TeduCoreApp.Application.Interfaces
{
    public interface ICommonService
    {
        FooterViewModel GetFooter();

        List<SlideViewModel> GetSlides(string groupAlias, int top = 5);

        SystemConfigViewModel GetSystemConfig(string code);
    }
}