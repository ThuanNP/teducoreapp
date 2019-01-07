using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;

namespace TeduCoreApp.Data.Entities
{
    [Table("AppUsers")]
    public class AppUser : IdentityUser<Guid>, IDateTracking, ISwitchable
    {
        public AppUser()
        {
        }

        public AppUser(Guid id, string fullName, decimal balance, string avatar, DateTime dateCreated, Status status)
        {
            Id = id;
            FullName = fullName;
            Balance = balance;
            Avatar = avatar;
            DateCreated = dateCreated;
            Status = status;
        }

        public string FullName { get; set; }
        public DateTime? BirthDay { set; get; }
        public decimal Balance { get; set; }
        public string Avatar { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public Status Status { get; set; }
    }
}