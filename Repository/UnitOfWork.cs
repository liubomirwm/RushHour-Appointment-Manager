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
    public class UnitOfWork
    {
        #region Fields/Members
        private RushHourContext context;
        private UsersRepository usersRepository;
        private ActivitiesRepository activitiesRepository;
        private AppointmentsRepository appointmentsRepository;
        #endregion

        #region Constructors
        public UnitOfWork()
        {
            this.context = new RushHourContext();
        }
        #endregion

        #region Properties
        public UsersRepository UsersRepository
        {
            get
            {
                if (this.usersRepository == null)
                {
                    this.usersRepository = new UsersRepository(this.context);
                }
                return this.usersRepository;
            }
        }

        public ActivitiesRepository ActivitiesRepository
        {
            get
            {
                if (this.activitiesRepository == null)
                {
                    this.activitiesRepository = new ActivitiesRepository(this.context);
                }
                return this.activitiesRepository;
            }
        }

        public AppointmentsRepository AppointmentsRepository
        {
            get
            {
                if (this.appointmentsRepository == null)
                {
                    this.appointmentsRepository = new AppointmentsRepository(this.context);
                }
                return this.appointmentsRepository;
            }
        }
        #endregion

        #region Methods
        public bool Save()
        {
            int objectsSavedCount = context.SaveChanges();
            bool hasSuccessfullySaved = true;
            if (objectsSavedCount == 0)
            {
                hasSuccessfullySaved = false;
            }
            return hasSuccessfullySaved;
        }
        #endregion
    }
}
