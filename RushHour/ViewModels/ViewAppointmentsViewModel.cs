using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class ViewAppointmentsViewModel
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start time")]

        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End date")]
        public DateTime EndDateTime { get; set; }

        [Required(ErrorMessage = "Is Cancelled is a required field")]
        [Display(Name = "Is Cancelled")]
        public bool IsCancelled { get; set; }

        public List<Activity> activities { get; set; }
    }
}