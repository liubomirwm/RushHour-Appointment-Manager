using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.ViewModels
{
    public class EditUserViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(100, ErrorMessage = "Email address cannot be greater than 100 characters.")]
        [EmailAddress]
        [Remote("IsExistingEmail", "Account", AdditionalFields = "acceptOwnEmail, UserId", ErrorMessage = "That email address is already registered in the system. Try with a different one.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Name cannot be more than 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You have to specify whether user will be admin")]
        [Display(Name = "Is Admin")]
        public bool IsAdmin { get; set; }
    }
}