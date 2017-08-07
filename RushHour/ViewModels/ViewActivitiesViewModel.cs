using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class ViewActivitiesViewModel
    {
        [Required(ErrorMessage = "Id is required.")]
        public int ActivityId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(250, ErrorMessage = "Name cannot be more than 250 characters long.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Duration is required.")]
        public double Duration { get; set; }
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }
    }
}