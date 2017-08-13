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
    public class ActivitiesController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult ViewActivities()
        {
            IEnumerable<Activity> activities = unitOfWork.ActivitiesRepository.GetAll();
            List<ViewActivitiesViewModel> viewModels = new List<ViewActivitiesViewModel>();
            foreach (Activity activity in activities)
            {
                ViewActivitiesViewModel viewModel = new ViewActivitiesViewModel();
                viewModel.ActivityId = activity.ActivityId;
                viewModel.Duration = activity.Duration;
                viewModel.Name = activity.Name;
                viewModel.Price = activity.Price;
                viewModels.Add(viewModel);
            }
            return View(viewModels);
        }

        public ActionResult AddActivity()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddActivity(AddActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Activity activity = new Activity();
                activity.ActivityId = 0;
                activity.Duration = viewModel.Duration;
                activity.Name = viewModel.Name;
                activity.Price = viewModel.Price;
                if (unitOfWork.ActivitiesRepository.GetAll().Any(a => a.Name == viewModel.Name))
                {
                    ModelState.AddModelError("ExistingActivity", "There is already an activity with the same name in the database");
                    return View(viewModel);
                }

                unitOfWork.ActivitiesRepository.Add(activity);
                bool hasSuccessfullySaved = unitOfWork.Save();
                if (hasSuccessfullySaved)
                {
                    TempData["SuccessfullMessage"] = "Activity added successfully!";
                    return RedirectToAction("ViewActivities", "Activities");
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a server error during the save of the activity. Try again!";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(viewModel);
        }

        public ActionResult EditActivity(int id)
        {
            Activity activity = unitOfWork.ActivitiesRepository.GetById(id);
            if (activity == null)
            {
                TempData["ErrorMessage"] = "There is no activity with this id";
                return RedirectToAction("Index", "Home");
            }
            EditActivityViewModel viewModel = new EditActivityViewModel();
            viewModel.ActivityId = activity.ActivityId;
            viewModel.Duration = activity.Duration;
            viewModel.Name = activity.Name;
            viewModel.Price = activity.Price;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditActivity(EditActivityViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                bool isExistingActivity = unitOfWork.ActivitiesRepository.GetAll(a => a.ActivityId == viewModel.ActivityId).Any();
                if (isExistingActivity == false)
                {
                    TempData["ErrorMessage"] = "There is no existing activity with that id. You can add one if you want.";
                    return RedirectToAction("ViewActivities", "Activities");
                }

                Activity dbActivity = unitOfWork.ActivitiesRepository.GetById(viewModel.ActivityId);
                dbActivity.ActivityId = viewModel.ActivityId;
                dbActivity.Duration = viewModel.Duration;
                dbActivity.Name = viewModel.Name;
                dbActivity.Price = viewModel.Price;
                unitOfWork.ActivitiesRepository.Edit(dbActivity);
                bool hasSuccessfullySaved = unitOfWork.Save();
                if (hasSuccessfullySaved)
                {
                    TempData["SuccessfullMessage"] = "Activity edited successfully";
                    return RedirectToAction("ViewActivities", "Activities");
                }
                else
                {
                    TempData["ErrorMessage"] = "Server error when trying to update the activity";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(viewModel);
        }

        public ActionResult DeleteActivity(int id)
        {
            Activity dbActivity = unitOfWork.ActivitiesRepository.GetById(id);
            if (dbActivity == null)
            {
                TempData["ErrorMessage"] = "There is no existing activity with that id";
                return RedirectToAction("ViewActivities", "Activities");
            }

            unitOfWork.ActivitiesRepository.Delete(dbActivity);
            bool hasSuccessfullySaved = unitOfWork.Save();
            if (hasSuccessfullySaved)
            {
                TempData["SuccessfullMessage"] = "Activity successfully deleted";
                return RedirectToAction("ViewActivities", "Activities");
            }
            else
            {
                TempData["ErrorMessage"] = "Server error during the activity deletion";
                return RedirectToAction("ViewActivities", "Activities");
            }
        }
    }
}