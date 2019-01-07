using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeduCoreApp.Application.ViewModels.System;
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Interfaces
{
    public interface IUserService
    {
        Task<bool> AddAsync(AppUserViewModel userViewModel);
        Task DeleteAsync(string id);
        Task<List<AppUserViewModel>> GetAllAsync();
        PagedResult<AppUserViewModel> GetAllPaggingAsync(string keyword, int page, int pageSize);
        Task<AppUserViewModel> GetByIdAsync(string Id);
        Task UpdateAsync(AppUserViewModel userViewModel);
    }
}
