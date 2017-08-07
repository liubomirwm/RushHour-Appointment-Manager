using DataAccess.Models;
using Repository;
using RushHour.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.Controllers
{
    public class UsersController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult AddUser()
        {
            return View();
        }

        public ActionResult ViewUsers()
        {
            IEnumerable<User> users = unitOfWork.UsersRepository.GetAll();
            List<ViewUsersViewModel> viewModels = new List<ViewUsersViewModel>();
            foreach (User user in users)
            {
                ViewUsersViewModel viewModel = new ViewUsersViewModel();
                viewModel.Email = user.Email;
                viewModel.IsAdmin = user.IsAdmin;
                viewModel.Name = user.Name;
                viewModel.UserId = user.UserId;
                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }

        public ActionResult EditUser(int id)
        {
            User dbUser = unitOfWork.UsersRepository.GetById(id);
            if (dbUser == null)
            {
                TempData["ErrorMessage"] = "There is no existing user with that id.";
                return RedirectToAction("Index", "Home");
            }

            EditUserViewModel viewModel = new EditUserViewModel();
            viewModel.UserId = dbUser.UserId;
            viewModel.Email = dbUser.Email;
            viewModel.IsAdmin = dbUser.IsAdmin;
            viewModel.Name = dbUser.Name;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditUser(EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User dbUser = unitOfWork.UsersRepository.GetById(viewModel.UserId);
                if (dbUser == null)
                {
                    TempData["ErrorMessage"] = "There is no existing user with that id.";
                    return RedirectToAction("Index", "Home");
                }

                dbUser.Email = viewModel.Email;
                dbUser.Name = viewModel.Name;
                dbUser.IsAdmin = viewModel.IsAdmin;
                unitOfWork.UsersRepository.Edit(dbUser);
                bool hasSuccessfullySaved = unitOfWork.Save();
                if (hasSuccessfullySaved)
                {
                    TempData["SuccessfullMessage"] = "User edited successfully.";
                    return RedirectToAction("ViewUsers", "Users");
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a server error during the edit of the user.";
                    return RedirectToAction("ViewUsers", "Users");
                }
            }

            return View(viewModel);
        }
    }
}