using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TeduCoreApp.Data.Enums;
using TeduCoreApp.Data.Interfaces;
using TeduCoreApp.infrastructure.SharedKernel;

namespace TeduCoreApp.Data.Entities
{
    [Table("Contacts")]
    public class Contact : DomainEntity<string>, ISwitchable
    {
        public Contact()
        {
        }

        public Contact(string id, string name, string email, string phone, string website, string address, string other, double? lng, double? lat, Status status)
        {
            Id = id;
            Name = name;
            Email = email;
            Phone = phone;
            Website = website;
            Address = address;
            Other = other;
            Lng = lng;
            Lat = lat;
            Status = status;
        }

        [Required, MaxLength(256)]
        public string Name { get; set; }

        [MaxLength(256)]
        public string Email { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [MaxLength(256)]
        public string Website { get; set; }

        [MaxLength(250)]
        public string Address { set; get; }

        public string Other { set; get; }

        public double? Lng { get; set; }

        public double? Lat { get; set; }

        public Status Status { get; set; }
    }
}