using DataAccess.Models;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Services
{
    public class UsersService
    {
        private IValidationDictionary validationDictionary;
        private UnitOfWork unitOfWork;

        public UsersService(IValidationDictionary validationDictionary, UnitOfWork unitOfWork)
        {
            this.validationDictionary = validationDictionary;
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<User> GetAll(Func<User, bool> filter = null)
        {
            return unitOfWork.UsersRepository.GetAll(filter);
        }

        public User GetById(int id)
        {
            User user = unitOfWork.UsersRepository.GetById(id);
            return user;
        }

        public bool IsValidModelState()
        {
            if (this.validationDictionary.IsValid)
            {
                return true;
            }

            return false;
        }

        public bool AddUser(User user)
        {
            unitOfWork.UsersRepository.Add(user);
            bool hasSuccessfullySaved = unitOfWork.Save();
            return hasSuccessfullySaved;
        }

        public bool Edit(User user)
        {
            unitOfWork.UsersRepository.Edit(user);
            bool hasSuccessfullyChanged = unitOfWork.Save();
            return hasSuccessfullyChanged;
        }

        public bool AuthenticateUser(string email, string password)
        {
            bool isValidCredentials = unitOfWork.UsersRepository.IsExistingEntity(u => u.Email == email && u.Password == password);
            if (isValidCredentials)
            {
                return true;
            }
            else
            {
                validationDictionary.AddModelError("wrongCredentials", "Wrong password or not existing account.");
                return false;
            }

        }

        public bool IsExistingEntity(Func<User, bool> filter)
        {
            return unitOfWork.UsersRepository.IsExistingEntity(filter);
        }

        public bool Delete(User dbUser)
        {
            unitOfWork.UsersRepository.Delete(dbUser);
            bool hasSuccessfullySaved = unitOfWork.Save();
            return hasSuccessfullySaved;
        }
    }
}
