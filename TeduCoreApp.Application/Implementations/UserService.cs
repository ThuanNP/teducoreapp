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
using TeduCoreApp.Utilities.Dtos;

namespace TeduCoreApp.Application.Implementations
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<bool> AddAsync(AppUserViewModel userViewModel)
        {
            //var user = new AppUser()
            //{
            //    UserName = userViewModel.UserName,
            //    Avatar = userViewModel.Avatar,
            //    Email = userViewModel.Email,
            //    FullName = userViewModel.FullName,
            //    PhoneNumber = userViewModel.PhoneNumber,
            //    Status = userViewModel.Status
            //};

            // Hack: user mapper alternate contructor of AppUser 
            var user = Mapper.Map<AppUserViewModel, AppUser>(userViewModel);
            var result = await _userManager.CreateAsync(user, userViewModel.Password);
            if (result.Succeeded && userViewModel.Roles.Count > 0)
            {
                var appUser = await _userManager.FindByNameAsync(user.UserName);
                if (appUser != null)
                {
                    await _userManager.AddToRolesAsync(appUser, userViewModel.Roles);
                }
            }
            return true;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<List<AppUserViewModel>> GetAllAsync()
        {
            return await _userManager.Users.ProjectTo<AppUserViewModel>().ToListAsync();
        }

        public PagedResult<AppUserViewModel> GetAllPaggingAsync(string keyword, int page, int pageSize)
        {
            var query = _userManager.Users;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x=>x.UserName.Contains(keyword) || x.FullName.Contains(keyword));
            }
            int totalRow = query.Count();
            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            var data = query.Select(u => new AppUserViewModel()
            {
                UserName = u.UserName,
                Avatar = u.Avatar,
                BirthDay = u.BirthDay.ToString(),
                Email = u.Email,
                FullName = u.FullName,
                Id = u.Id,
                PhoneNumber = u.PhoneNumber,
                Status = u.Status,
                DateCreated = u.DateCreated,
                DateModified = u.DateModified
            }).ToList();
            var paginationSet = new PagedResult<AppUserViewModel>()
            {
                Results = data,
                CurrentPage = page,
                RowCount = totalRow,
                PageSize = pageSize
            };
            return paginationSet;
        }

        public async Task<AppUserViewModel> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var roles = await _userManager.GetRolesAsync(user);
            var userVM = Mapper.Map<AppUser, AppUserViewModel>(user);
            userVM.Roles = roles.ToList();
            return userVM;
        }

        public async Task UpdateAsync(AppUserViewModel userViewModel)
        {
            var user = await _userManager.FindByIdAsync(userViewModel.Id.ToString());
            //Remove current roles in database
            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.AddToRolesAsync(user, userViewModel.Roles.Except(currentRoles).ToArray());
            if (result.Succeeded)
            {
                string[] needRemoveRoles = currentRoles.Except(userViewModel.Roles).ToArray();
                await _userManager.RemoveFromRolesAsync(user, needRemoveRoles);
                //Update user detail
                user.FullName = userViewModel.FullName;
                user.Avatar = userViewModel.Avatar;
               
                user.Email = userViewModel.Email;
                user.PhoneNumber = userViewModel.PhoneNumber;
                user.Status = userViewModel.Status;
                await _userManager.UpdateAsync(user);
            }
        }
    }
}
