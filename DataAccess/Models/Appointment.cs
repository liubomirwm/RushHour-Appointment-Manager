using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        public int UserId { get; set; }

        public User User { get; set; }
        public ICollection<Activity> Activities { get; set; }
    }
}
