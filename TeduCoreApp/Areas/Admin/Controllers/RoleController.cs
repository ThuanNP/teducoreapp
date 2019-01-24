using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;

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

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _roleService.GetAllPaggingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var model = await _roleService.GetByIdAsync(id);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppRoleViewModel appRoleViewModel)
        {
            if (ModelState.IsValid)
            {
                if (appRoleViewModel.Id == null)
                {
                    await _roleService.AddAsync(appRoleViewModel);
                }
                else
                {
                    await _roleService.UpdateAsync(appRoleViewModel);
                }
            }
            else
            {
                IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(modelErrors);
            }
            return new OkObjectResult(appRoleViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (ModelState.IsValid)
            {
                var result = await _roleService.DeleteAsync(id);
                return new OkObjectResult(id);
            }
            else
            {
                IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(modelErrors);
            }
        }

        [HttpGet]
        public IActionResult GetListAllFunction(Guid roleId)
        {
            var functions = _roleService.GetListFunctionWithRole(roleId);
            return new OkObjectResult(functions);
        }

        [HttpPost]
        public IActionResult SavePermission(List<PermissionViewModel> permissionViewModels, Guid roleId)
        {
            _roleService.SavePermission(permissionViewModels, roleId);
            return new OkResult();
        }

        #endregion Ajax api
    }
}