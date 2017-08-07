using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class ViewUsersViewModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(100, ErrorMessage = "Email address cannot be greater than 100 characters.")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [MaxLength(200, ErrorMessage = "Name cannot be more than 200 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You have to specify whether user will be admin")]
        public bool IsAdmin { get; set; }
    }
}