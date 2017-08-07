using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class Activity
    {
        public int ActivityId { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        public double Duration { get; set; }

        [Required]
        public decimal Price { get; set; }

        public ICollection<Appointment> Appointments { get; set; }
    }
}
