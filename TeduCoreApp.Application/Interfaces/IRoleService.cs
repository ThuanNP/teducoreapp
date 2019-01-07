using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.ViewModels.System;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IRoleService
    {
        Task<List<AppRoleViewModel>> GetAllAsync();
    }
}
