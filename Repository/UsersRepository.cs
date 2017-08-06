using DataAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class UsersRepository : BaseRepository<User>
    {
        public UsersRepository(RushHourContext context) : base(context)
        {}
    }
}
