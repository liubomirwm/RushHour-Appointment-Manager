using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Models;

namespace Services
{
    public class ActivitiesService
    {
        private IValidationDictionary validationDictionary;
        private UnitOfWork unitOfWork;

        public ActivitiesService(IValidationDictionary validationDictionary, UnitOfWork unitOfWork)
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

            return false;
        }

        public IEnumerable<Activity> GetAll(Func<Activity, bool> filter = null)
        {
            return unitOfWork.ActivitiesRepository.GetAll(filter);
        }

        public bool IsExistingActivity(Func<Activity, bool> filter)
        {
            if (unitOfWork.ActivitiesRepository.IsExistingEntity(filter))
            {
                return true;
            }

            return false;
        }

        public bool Add(Activity activity)
        {
            if (IsExistingActivity(a => a.Name == activity.Name))
            {
                validationDictionary.AddModelError("ExistingActivity", "There is already an activity with the same name in the database");
                bool hasSuccessfullySaved = false;
                return hasSuccessfullySaved;
            }
            else
            {
                unitOfWork.ActivitiesRepository.Add(activity);
                bool hasSuccessfullySaved = unitOfWork.Save();
                return hasSuccessfullySaved;
            }
        }

        public Activity GetById(int id)
        {
            return unitOfWork.ActivitiesRepository.GetById(id);
        }

        public bool Edit(Activity dbActivity)
        {
            unitOfWork.ActivitiesRepository.Edit(dbActivity);
            bool hasSuccessfullySaved = unitOfWork.Save();
            return hasSuccessfullySaved;
        }

        public bool Delete(Activity dbActivity)
        {
            unitOfWork.ActivitiesRepository.Delete(dbActivity);
            bool hasSuccessfullySaved = unitOfWork.Save();
            return hasSuccessfullySaved;
        }
    }
}
