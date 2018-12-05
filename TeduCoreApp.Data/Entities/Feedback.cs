using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Feedback")]
    public class Feedback : DomainEntity<int>, ISwitchable, IDateTracking
    {
        public Feedback()
        {
        }

        public Feedback(int id, string name, string email, string phone, string content, Status status, DateTime dateCreated, DateTime dateModified)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            Content = content;
            Status = status;
            DateCreated = dateCreated;
            DateModified = dateModified;
        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(256)]
        public string Phone { get; set; }

        public string Content { get; set; }
        public Status Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}