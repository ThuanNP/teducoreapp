using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;

namespace TeduCoreApp.Application.Implementations
{
    public class RoleService : IRoleService
    {
        private RoleManager<AppRole> _roleManager;
        private readonly IFunctionRepository _functionRepository;
        private readonly IPermissionRepository _permissionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RoleService(RoleManager<AppRole> roleManager, IFunctionRepository functionRepository, IPermissionRepository permissionRepository, IUnitOfWork unitOfWork)
        {
            _roleManager = roleManager;
            _functionRepository = functionRepository;
            _permissionRepository = permissionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<AppRoleViewModel>> GetAllAsync()
        {
            return await _roleManager.Roles.ProjectTo<AppRoleViewModel>().ToListAsync();
        }
    }
}
