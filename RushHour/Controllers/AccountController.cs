using RushHour.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Models;
using Repository;
using RushHour.Helpers;
using RushHour.Attributes;
using RushHour.Enums;
using Services;

namespace RushHour.Controllers
{
    public class AccountController : Controller
    {
        public UsersService usersService;

        public AccountController()
        {
            usersService = new UsersService(new ModelStateWrapper(this.ModelState), new UnitOfWork());
        }

        [CustomAuthorize(CustomAuthorizeEnum.AnonymousUser)]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(CustomAuthorizeEnum.AnonymousUser)]
        public ActionResult Login(LoginViewModel viewModel)
        {
            if (usersService.IsValidModelState())
            {
                if (usersService.AuthenticateUser(viewModel.Email, viewModel.Password))
                {
                    User dbUser = usersService.GetAll(u => u.Email == viewModel.Email && u.Password == viewModel.Password).FirstOrDefault();
                    LoginUserSession.Current.SetCurrentUser(dbUser.UserId, dbUser.Email, dbUser.Name, dbUser.IsAdmin);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(viewModel);
                }
            }

            return View(viewModel);
        }

        [CustomAuthorize(CustomAuthorizeEnum.AnonymousUser)]
        public ActionResult Register() //TODO: Add phone
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(CustomAuthorizeEnum.AnonymousUser)]
        public ActionResult Register(RegisterViewModel viewModel)
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
                    TempData["SuccessfullMessage"] = "Registration successfull. You can now login.";
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a server error during the registration. Please try again!";
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(viewModel);
            }
        }

        [CustomAuthorize(CustomAuthorizeEnum.Admin | CustomAuthorizeEnum.NonAdmin)]
        public ActionResult Logout()
        {
            LoginUserSession.Current.Logout();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        [CustomAuthorize(CustomAuthorizeEnum.Everyone)]
        public JsonResult IsExistingEmail(string Email, bool acceptOwnEmail = false, int UserId = 0) //You better don't change UserId to userId ;)
        {
            if (acceptOwnEmail && LoginUserSession.Current.IsAdmin)
            {
                string currentEmail = usersService.GetById(UserId).Email;
                bool isExistingEmail = usersService.IsExistingEntity(u => u.Email == Email && Email != currentEmail);
                return Json(!isExistingEmail, JsonRequestBehavior.AllowGet);
            }
            else if (acceptOwnEmail)
            {
                bool isExistingEmail = usersService.IsExistingEntity(u => u.Email == Email && Email != LoginUserSession.Current.Email);
                return Json(!isExistingEmail, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool isExistingEmail = usersService.IsExistingEntity(u => u.Email == Email);
                return Json(!isExistingEmail, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("EditProfile")]
        [CustomAuthorize(CustomAuthorizeEnum.Admin | CustomAuthorizeEnum.NonAdmin)]
        public ActionResult Edit()
        {
            User dbUser = new User();
            dbUser = usersService.GetById(LoginUserSession.Current.UserId);
            EditProfileViewModel viewModel = new EditProfileViewModel();
            viewModel.Email = dbUser.Email;
            viewModel.Name = dbUser.Name;
            viewModel.UserId = LoginUserSession.Current.UserId;
            return View("Edit", viewModel);
        }

        [HttpPost]
        [ActionName("EditProfile")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(CustomAuthorizeEnum.Admin | CustomAuthorizeEnum.NonAdmin)]
        public ActionResult Edit(EditProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                User user = usersService.GetById(LoginUserSession.Current.UserId);
                user.Email = viewModel.Email;
                user.Name = viewModel.Name;
                bool hasSuccessfullyEdited = true;
                try
                {
                    hasSuccessfullyEdited = usersService.Edit(user);
                }
                catch (System.Data.SqlClient.SqlException)
                {
                    hasSuccessfullyEdited = false;
                }

                if (hasSuccessfullyEdited)
                {
                    LoginUserSession currentSession = LoginUserSession.Current;
                    currentSession.Email = viewModel.Email;
                    currentSession.Name = viewModel.Name;
                    TempData["SuccessfullMessage"] = "Profile edited successfully";
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a server error during the edit";
                }

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(viewModel);
            }

        }
    }
}