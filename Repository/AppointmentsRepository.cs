using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Repository
{
    public class AppointmentsRepository : BaseRepository<Appointment>
    {
        public AppointmentsRepository(RushHourContext context) : base(context)
        {
        }
    }
}
