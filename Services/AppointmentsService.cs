using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace Services
{
    public class AppointmentsService
    {
        private IValidationDictionary validationDictionary;
        private UnitOfWork unitOfWork;

        public AppointmentsService(IValidationDictionary validationDictionary, UnitOfWork unitOfWork)
        {
            this.validationDictionary = validationDictionary;
            this.unitOfWork = unitOfWork;
        }

        public bool IsValidModelState()
        {
            if (this.validationDictionary.IsValid)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<Activity> GetAll(Func<object, bool> filter)
        {
            throw new NotImplementedException();
        }

        public bool Add(Appointment appointment)
        {
            unitOfWork.AppointmentsRepository.Add(appointment);
            bool hasSuccessfullySaved = unitOfWork.Save();
            return hasSuccessfullySaved;
        }

        public IEnumerable<Appointment> GetAll(Func<Appointment, bool> filter = null)
        {
            return unitOfWork.AppointmentsRepository.GetAll(filter);
        }

        public Appointment GetById(int id)
        {
            return unitOfWork.AppointmentsRepository.GetById(id);
        }

        public bool Edit(Appointment dbAppointment)
        {
            unitOfWork.AppointmentsRepository.Edit(dbAppointment);
            bool hasSuccessfullySaved = unitOfWork.Save();
            return hasSuccessfullySaved;
        }
    }
}
