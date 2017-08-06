﻿using RushHour.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataAccess.Models;
using Repository;
using RushHour.Helpers;

namespace RushHour.Controllers
{
    public class AccountController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel viewModel)
        {
            DataAccess.Models.User dbUser = unitOfWork.UsersRepository.GetAll(u => u.Email == viewModel.Email && u.Password == viewModel.Password).FirstOrDefault();
            if (dbUser != null)
            {
                LoginUserSession.Current.SetCurrentUser(dbUser.UserId, dbUser.Email, dbUser.Name, dbUser.IsAdmin);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                ModelState.AddModelError("wrongCredentials", "Wrong password or not existing account.");
                return View(viewModel);
            }
        }

        public ActionResult Register()
        {
            RegisterViewModel viewModel = new RegisterViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Register(RegisterViewModel viewModel)
        {
            UnitOfWork unitOfWork = new UnitOfWork();

            if (ModelState.IsValid)
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
                    unitOfWork.UsersRepository.Add(user);
                    hasSuccessfullySaved = unitOfWork.Save();
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

        public ActionResult Logout()
        {
            LoginUserSession.Current.Logout();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public JsonResult IsExistingEmail(string Email, bool acceptOwnEmail = false)
        {
            if (acceptOwnEmail)
            {
                bool isExistingEmail = unitOfWork.UsersRepository.GetAll().Any(u => u.Email == Email && Email != LoginUserSession.Current.Email);
                return Json(!isExistingEmail, JsonRequestBehavior.AllowGet);
            }
            else
            {
                bool isExistingEmail = unitOfWork.UsersRepository.GetAll().Any(u => u.Email == Email);
                return Json(!isExistingEmail, JsonRequestBehavior.AllowGet);
            }
        }

        [ActionName("EditProfile")]
        public ActionResult Edit()
        {
            User dbUser = new User();
            dbUser = unitOfWork.UsersRepository.GetById(LoginUserSession.Current.UserId);
            EditProfileViewModel viewModel = new EditProfileViewModel();
            viewModel.Email = dbUser.Email;
            viewModel.Name = dbUser.Name;
            return View("Edit", viewModel);
        }

        [HttpPost]
        [ActionName("EditProfile")]
        public ActionResult Edit(EditProfileViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                DataAccess.Models.User user = unitOfWork.UsersRepository.GetById(LoginUserSession.Current.UserId);
                user.Email = viewModel.Email;
                user.Name = viewModel.Name;
                unitOfWork.UsersRepository.Edit(user);
                bool hasSuccessfullyEdited = true;
                try
                {
                    hasSuccessfullyEdited = unitOfWork.Save();
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