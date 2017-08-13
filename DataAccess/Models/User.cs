using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(60)]
        public string Password { get; set; } //TODO: Add ability to change password after password hashing is done.

        [Required]
        [MaxLength(200)]
        public string Name { get; set; }

        [Required]
        public bool IsAdmin { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }

    }
}
