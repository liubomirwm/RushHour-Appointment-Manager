using DataAccess.Models;
using Repository;
using RushHour.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RushHour.Attributes;
using Services;

namespace RushHour.Controllers
{
    [CustomAuthorize(Enums.CustomAuthorizeEnum.Admin)]
    public class UsersController : Controller
    {
        private UsersService usersService;
        public UsersController()
        {
            this.usersService = new UsersService(new ModelStateWrapper(this.ModelState), new UnitOfWork());
        }

        public ActionResult AddUser()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUser(RegisterViewModel viewModel)
        {
            if (usersService.IsValidModelState())
            {
                User user = new User()
                {
                    Email = viewModel.Email,
                    Password = viewModel.Password,
                    Name = viewModel.Name
                };

                bool hasSuccessfullySaved = true;
                try
                {
                    hasSuccessfullySaved = usersService.AddUser(user);
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    hasSuccessfullySaved = false;
                }
                if (hasSuccessfullySaved)
                {
                    TempData["SuccessfullMessage"] = "User added successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a server error during the registration. Please try again!";
                }

                return RedirectToAction("ViewUsers", "Users");
            }
            else
            {
                return View(viewModel);
            }
        }

        public ActionResult ViewUsers()
        {
            IEnumerable<User> users = usersService.GetAll();
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
            User dbUser = usersService.GetById(id);
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
            if (usersService.IsValidModelState())
            {
                User dbUser = usersService.GetById(viewModel.UserId);
                if (dbUser == null)
                {
                    TempData["ErrorMessage"] = "There is no existing user with that id.";
                    return RedirectToAction("Index", "Home");
                }

                dbUser.Email = viewModel.Email;
                dbUser.Name = viewModel.Name;
                dbUser.IsAdmin = viewModel.IsAdmin;
                bool hasSuccessfullySaved = usersService.Edit(dbUser);
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

        public ActionResult DeleteUser(int id)
        {
            User dbUser = usersService.GetById(id);
            if (dbUser == null)
            {
                TempData["ErrorMessage"] = "No user with that id exists";
                return RedirectToAction("Index", "Home");
            }

            bool hasSuccessfullySaved = usersService.Delete(dbUser);
            if (hasSuccessfullySaved)
            {
                TempData["SuccessfullMessage"] = "User deleted successfully";
                return RedirectToAction("ViewUsers", "Users");
            }
            else
            {
                TempData["ErrorMessage"] = "There was a server error while trying to delete the user. Try again.";
                return RedirectToAction("Index", "Home");
            }
        }
    }
}