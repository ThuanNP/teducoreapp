using System;
using System.ComponentModel.DataAnnotations;

namespace TeduCoreApp.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Full name required", AllowEmptyStrings = false)]
        [Display(Name = "Full name")]
        public string FullName { set; get; }

        [Display(Name = "DOB")]
        [DataType(DataType.Date)]
        public DateTime? BirthDay { set; get; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Address")]
        public string Address { get; set; }

        //[DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        public string PhoneNumber { set; get; }

        [DataType(DataType.ImageUrl)]
        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
    }
}