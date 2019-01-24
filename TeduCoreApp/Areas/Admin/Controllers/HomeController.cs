using Microsoft.AspNetCore.Mvc;
using TeduCoreApp.Extensions;

namespace TeduCoreApp.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            string email = User.GetSpecificClaim("Email");
            return View();
        }
    }
}