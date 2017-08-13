using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RushHour.ViewModels
{
    public class EditAppointmentViewModel
    {
        public int AppointmentId { get; set; }

        [Required(ErrorMessage = "Start Time is required.")]
        [Display(Name = "Start Time")]
        public DateTime StartDateTime { get; set; }

        [Required(ErrorMessage = "End Time is required")]
        [Display(Name = "End Time")]
        public DateTime EndDateTime { get; set; }

        public List<ActivityRow> activityRows;
        public List<int> CheckedRows { get; set; }
    }
}