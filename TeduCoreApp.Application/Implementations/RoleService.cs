using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeduCoreApp.Application.Interfaces;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Data.Entities;
using TeduCoreApp.Data.IRepositories;
using TeduCoreApp.infrastructure.Interfaces;
using TeduCoreApp.Utilities.Dtos;

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

        public async Task<AppRoleViewModel> GetByIdAsync(Guid id)
        {
            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            var model = Mapper.Map<AppRole, AppRoleViewModel>(appRole);
            return model;
        }

        public PagedResult<AppRoleViewModel> GetAllPaggingAsync(string keyword, int page, int pageSize)
        {
            var query = _roleManager.Roles;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.ProjectTo<AppRoleViewModel>().ToList();
            var paginationSet = new PagedResult<AppRoleViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }
        
        public async Task<IdentityResult> AddAsync(AppRoleViewModel appRoleViewModel)
        {
            var appRole = Mapper.Map<AppRoleViewModel, AppRole>(appRoleViewModel);
            return await _roleManager.CreateAsync(appRole);
        }

        public async Task<IdentityResult> UpdateAsync(AppRoleViewModel appRoleViewModel)
        {
            var appRole = Mapper.Map<AppRoleViewModel, AppRole>(appRoleViewModel);
            return await _roleManager.UpdateAsync(appRole);
        }

        public async Task<IdentityResult> DeleteAsync(Guid id)
        {
            var appRole = await _roleManager.FindByIdAsync(id.ToString());
            return await _roleManager.DeleteAsync(appRole);
        }

        public List<PermissionViewModel> GetListFunctionWithRole(Guid roleId)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();

            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId into fp
                        from p in fp.DefaultIfEmpty()
                        where p != null && p.RoleId == roleId
                        let canRead = p != null ? p.CanRead : false
                        let canCreate = p != null ? p.CanCreate : false
                        let candUpdate = p != null ? p.CanUpdate : false
                        let canDelete = p != null ? p.CanDelete : false
                        select new PermissionViewModel()
                        {
                            RoleId = roleId,
                            FunctionId = f.Id,
                            CanRead = canRead,
                            CanCreate = canCreate,
                            CanUpdate = candUpdate,
                            CanDelete = canDelete
                        };
            return query.ToList();
        }

        public void UpdatePermission(List<PermissionViewModel> permissionViewModels, Guid roleId)
        {
            var permissions = Mapper.Map<List<PermissionViewModel>, List<Permission>>(permissionViewModels);
            var oldPermissions = _permissionRepository.FindAll().Where(r => r.RoleId.Equals(roleId)).ToList();

            if (oldPermissions.Count>0)
            {
                _permissionRepository.RemoveMultiple(oldPermissions);
            }
            foreach (var permission in permissions)
            {
                _permissionRepository.Add(permission);
            }
        }

        public void Save() => _unitOfWork.Commit();

        public void Dispose() => GC.SuppressFinalize(this);

        public Task<bool> CheckPermission(string functionId, string action, string[] roles)
        {
            var functions = _functionRepository.FindAll();
            var permissions = _permissionRepository.FindAll();
            var query = from f in functions
                        join p in permissions on f.Id equals p.FunctionId
                        join r in _roleManager.Roles on p.RoleId equals r.Id
                        where roles.Contains(r.Name) && f.Id.Equals(functionId)
                        && ((p.CanCreate && action == "Create")
                        || (p.CanUpdate && action == "Update")
                        || (p.CanDelete && action == "Delete")
                        || (p.CanRead && action == "Read"))
                        select p;
            return query.AnyAsync();
        }
    }
}
