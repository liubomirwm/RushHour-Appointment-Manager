using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class AddActivityViewModel
    {
        public int ActivityId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(250, ErrorMessage = "Name cannot be more than 250 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Display(Name = "Duration (in Minutes)")]
        public double Duration { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}