using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeduCoreApp.Data.Entities
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole<Guid>
    {
        public AppRole() : base()
        {
        }

        public AppRole(string name, string description) : base(name) => Description = description;

        [StringLength(250)]
        public string Description { get; set; }
    }
}