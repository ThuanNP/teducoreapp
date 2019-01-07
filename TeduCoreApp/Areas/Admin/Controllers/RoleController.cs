using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;

namespace TeduCoreApp.Areas.Admin.Controllers
{
    public class RoleController : BaseController
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region Ajax api

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var model = await _roleService.GetAllAsync();
            return new OkObjectResult(model);
        }

        #endregion
    }
}