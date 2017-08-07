using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess;

namespace Repository
{
    public class ActivitiesRepository : BaseRepository<Activity>
    {
        public ActivitiesRepository(RushHourContext context) : base(context)
        {}
    }
}
