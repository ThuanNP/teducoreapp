using System;
using System.ComponentModel.DataAnnotations;

namespace TeduCoreApp.Application.ViewModels.System
{
    public class PermissionViewModel
    {
        public int Id { get; set; }
        public Guid RoleId { get; set; }

        [Required, StringLength(128)]
        public string FunctionId { get; set; }

        public bool CanCreate { set; get; }
        public bool CanRead { set; get; }
        public bool CanUpdate { set; get; }
        public bool CanDelete { set; get; }

        public virtual AppRoleViewModel AppRole { get; set; }

        public virtual FunctionViewModel Function { get; set; }
    }
}