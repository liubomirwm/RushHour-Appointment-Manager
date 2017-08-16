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
    public class ActivitiesController : Controller
    {
        ActivitiesService activitiesService;
        public ActivitiesController()
        {
            this.activitiesService = new ActivitiesService(new ModelStateWrapper(this.ModelState), new UnitOfWork());
        }

        public ActionResult ViewActivities()
        {
            IEnumerable<Activity> activities = activitiesService.GetAll();
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
            if (activitiesService.IsValidModelState())
            {
                Activity activity = new Activity();
                activity.ActivityId = 0;
                activity.Duration = viewModel.Duration;
                activity.Name = viewModel.Name;
                activity.Price = viewModel.Price;
                if (activitiesService.IsExistingActivity(a => a.Name == activity.Name))
                {
                    return View(viewModel);
                }

                bool hasSuccessfullySaved = activitiesService.Add(activity);
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
            Activity activity = activitiesService.GetById(id);
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
            if (activitiesService.IsValidModelState())
            {
                bool isExistingActivity = activitiesService.IsExistingActivity(a => a.ActivityId == viewModel.ActivityId);
                if (isExistingActivity == false)
                {
                    TempData["ErrorMessage"] = "There is no existing activity with that id. You can add one if you want.";
                    return RedirectToAction("ViewActivities", "Activities");
                }

                Activity dbActivity = activitiesService.GetById(viewModel.ActivityId);
                dbActivity.ActivityId = viewModel.ActivityId;
                dbActivity.Duration = viewModel.Duration;
                dbActivity.Name = viewModel.Name;
                dbActivity.Price = viewModel.Price;
                bool hasSuccessfullySaved = activitiesService.Edit(dbActivity);
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
            Activity dbActivity = activitiesService.GetById(id);
            if (dbActivity == null)
            {
                TempData["ErrorMessage"] = "There is no existing activity with that id";
                return RedirectToAction("ViewActivities", "Activities");
            }

            bool hasSuccessfullySaved = activitiesService.Delete(dbActivity);
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