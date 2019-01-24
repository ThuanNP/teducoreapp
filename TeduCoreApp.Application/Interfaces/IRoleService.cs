using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<AppRoleViewModel>> GetAllAsync();

        Task<AppRoleViewModel> GetByIdAsync(Guid id);

        PagedResult<AppRoleViewModel> GetAllPaggingAsync(string keyword, int page, int pageSize);

        Task<IdentityResult> AddAsync(AppRoleViewModel appRoleViewModel);

        Task<IdentityResult> UpdateAsync(AppRoleViewModel appRoleViewModel);

        Task<IdentityResult> DeleteAsync(Guid id);

        List<PermissionViewModel> GetListFunctionWithRole(Guid roleId);

        void SavePermission(List<PermissionViewModel> permissionViewModels, Guid roleId);
    }
}
