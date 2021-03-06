﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Authorization;

namespace TeduCoreApp.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IAuthorizationService _authorizationService;

        public UserController(IUserService userService, IAuthorizationService authorizationService)
        {
            _userService = userService;
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _authorizationService.AuthorizeAsync(User, "USER", Operations.Read);
            if (result.Succeeded == false)
            {
                return new RedirectResult("/Admin/Login/Index");
            }
            return View();
        }

        #region Ajax Api

        [HttpGet]
        public IActionResult GetAll()
        {
            var model = _userService.GetAllAsync();
            return new OkObjectResult(model);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var model = await _userService.GetByIdAsync(id);
            return new OkObjectResult(model);
        }

        [HttpGet]
        public IActionResult GetAllPaging(string keyword, int page, int pageSize)
        {
            var model = _userService.GetAllPaggingAsync(keyword, page, pageSize);
            return new OkObjectResult(model);
        }

        [HttpPost]
        public async Task<IActionResult> SaveEntity(AppUserViewModel userViewModel)
        {
            if (ModelState.IsValid)
            {
                if (userViewModel.Id == null)
                {
                    await _userService.AddAsync(userViewModel);
                }
                else
                {
                    await _userService.UpdateAsync(userViewModel);
                }
            }
            else
            {
                IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(modelErrors);
            }
            return new OkObjectResult(userViewModel);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            if (ModelState.IsValid)
            {
                await _userService.DeleteAsync(id);
                return new OkObjectResult(id);
            }
            else
            {
                IEnumerable<ModelError> modelErrors = ModelState.Values.SelectMany(v => v.Errors);
                return new BadRequestObjectResult(modelErrors);
            }
        }

        #endregion Ajax Api
    }
}