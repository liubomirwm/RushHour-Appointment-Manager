using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.ViewModels
{
    public class EditProfileViewModel
    {
        [StringLength(100, ErrorMessage = "Email should be less than 100 characters long.")]
        [EmailAddress]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Email address is required.")]
        [Remote("IsExistingEmail", "Account", AdditionalFields = "acceptOwnEmail", ErrorMessage = "That email address is already registered in the system. Try with a different one.")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "Name cannot be more than 200 characters long.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}