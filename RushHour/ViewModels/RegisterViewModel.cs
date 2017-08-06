using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.ViewModels
{
    public class RegisterViewModel
    {
        [StringLength(100, ErrorMessage = "Email should be less than 100 characters long.")]
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email address is required.")]
        [Remote("IsExistingEmail", "Account", ErrorMessage = "That email address is already registered in the system. Try with a different one.")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Password is required")]
        [StringLength(60, ErrorMessage = "Password should be less than 60 characters long.")]
        public string Password { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "You need to type the password again.")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "The passwords must match.")]
        [Display(Name = "Repeat Password")]
        [DataType(DataType.Password)]
        public string RepeatPassword { get; set; }

        [StringLength(200, ErrorMessage = "Name cannot be more than 200 characters long.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}