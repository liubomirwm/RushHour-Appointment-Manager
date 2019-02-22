using DataAccess.Models;
using Repository;
using RushHour.Attributes;
using RushHour.Helpers;
using RushHour.ViewModels;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.Controllers
{
    public class AppointmentsController : Controller
    {
        private AppointmentsService appointmentsService;
        private ActivitiesService activitiesService;

        public AppointmentsController()
        {
            UnitOfWork unitOfWork = new UnitOfWork();
            this.appointmentsService = new AppointmentsService(new ModelStateWrapper(this.ModelState), unitOfWork);
            this.activitiesService = new ActivitiesService(new ModelStateWrapper(this.ModelState), unitOfWork);
        }

        [CustomAuthorize(Enums.CustomAuthorizeEnum.NonAdmin)]
        public ActionResult AddAppointment()
        {
            IEnumerable<Activity> dbActivities = activitiesService.GetAll();

            List<ActivityRow> activityRows = new List<ActivityRow>();
            AddAppointmentViewModel viewModel = new AddAppointmentViewModel();
            DateTime currentTime = DateTime.Now;
            viewModel.StartDateTime = currentTime;
            viewModel.EndDateTime = currentTime;

            foreach (Activity activity in dbActivities)
            {
                ActivityRow activityRow = new ActivityRow();
                activityRow.activity = activity;
                activityRow.isChecked = false;
                activityRows.Add(activityRow);
            }
            viewModel.activityRows = activityRows;
            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Enums.CustomAuthorizeEnum.NonAdmin)]
        public ActionResult AddAppointment(AddAppointmentViewModel viewModel)
        {
            if (appointmentsService.IsValidModelState())
            {
                Appointment appointment = new Appointment();

                double summedMinutes = 0;
                List<Activity> activities = activitiesService.GetAll(a => viewModel.CheckedRows.Contains(a.ActivityId)).ToList();
                foreach (Activity activity in activities)
                {
                    summedMinutes += activity.Duration;
                    appointment.Activities.Add(activity);
                }

                DateTime startDateTime = viewModel.StartDateTime;
                appointment.StartDateTime = startDateTime;
                TimeSpan summedDuration = TimeSpan.FromMinutes(summedMinutes);
                DateTime endDateTime = startDateTime.Add(summedDuration);
                appointment.EndDateTime = endDateTime;
                appointment.UserId = LoginUserSession.Current.UserId;
                bool hasSavedSuccessfully = appointmentsService.Add(appointment);
                if (hasSavedSuccessfully)
                {
                    TempData["SuccessfullMessage"] = "Appointment added successfully";
                    return RedirectToAction("ViewAppointments", "Appointments");
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a server error while adding the appointment.";
                    return RedirectToAction("Index", "Home");
                }
            }

            return View(viewModel);
        }

        [CustomAuthorize(Enums.CustomAuthorizeEnum.Admin | Enums.CustomAuthorizeEnum.NonAdmin)]
        public ActionResult ViewAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();
            if (LoginUserSession.Current.IsAdmin)
            {
                appointments = appointmentsService.GetAll().ToList();
            }
            else
            {
                appointments = appointmentsService.GetAll(a => a.UserId == LoginUserSession.Current.UserId).ToList();
            }

            List<ViewAppointmentsViewModel> viewModels = new List<ViewAppointmentsViewModel>();
            foreach (Appointment appointment in appointments)
            {
                ViewAppointmentsViewModel viewModel = new ViewAppointmentsViewModel();
                viewModel.AppointmentId = appointment.AppointmentId;
                viewModel.activities = appointment.Activities.ToList();
                viewModel.StartDateTime = appointment.StartDateTime;
                viewModel.EndDateTime = appointment.EndDateTime;
                viewModel.IsCancelled = appointment.IsCancelled;
                viewModels.Add(viewModel);
            }

            return View(viewModels);
        }

        [CustomAuthorize(Enums.CustomAuthorizeEnum.NonAdmin)]
        public ActionResult EditAppointment(int id)
        {
            Appointment dbAppointment = appointmentsService.GetById(id);
            var redirectResult = GetRedirectIfAppointmentNotOwned(dbAppointment);
            if (redirectResult != null)
            {
                return redirectResult;
            }

            List<ActivityRow> activityRows = GetActivityRowsForAllActivities();
            EditAppointmentViewModel viewModel = CreateViewModelFromAppointment(dbAppointment);
            MarkActivityRowsForAppointmentActivitiesAsChecked(activityRows, dbAppointment.Activities);
            viewModel.activityRows = activityRows;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize(Enums.CustomAuthorizeEnum.NonAdmin)]
        public ActionResult EditAppointment(EditAppointmentViewModel viewModel)
        {
            if (appointmentsService.IsValidModelState())
            {
                Appointment dbAppointment = appointmentsService.GetById(viewModel.AppointmentId);
                dbAppointment.StartDateTime = viewModel.StartDateTime;
                double summedMinutes = 0;
                dbAppointment.Activities.Clear();
                List<Activity> activities = activitiesService.GetAll(a => viewModel.CheckedRows.Contains(a.ActivityId)).ToList();
                foreach (Activity activity in activities)
                {
                    summedMinutes += activity.Duration;
                    dbAppointment.Activities.Add(activity);
                }

                DateTime startDateTime = viewModel.StartDateTime;
                dbAppointment.StartDateTime = startDateTime;
                TimeSpan summedDuration = TimeSpan.FromMinutes(summedMinutes);
                DateTime endDateTime = startDateTime.Add(summedDuration);
                dbAppointment.EndDateTime = endDateTime;
                dbAppointment.UserId = LoginUserSession.Current.UserId;
                bool hasSavedSuccessfully = appointmentsService.Edit(dbAppointment);
                if (hasSavedSuccessfully)
                {
                    TempData["SuccessfullMessage"] = "Appointment added successfully";
                    return RedirectToAction("ViewAppointments", "Appointments");
                }
                else
                {
                    TempData["ErrorMessage"] = "There was a server error while adding the appointment.";
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(viewModel);
        }

        [CustomAuthorize(Enums.CustomAuthorizeEnum.NonAdmin)]
        public ActionResult CancelAppointment(int id)
        {
            Appointment dbAppointment = appointmentsService.GetById(id);
            dbAppointment.IsCancelled = true;
            bool hasSuccessfullySaved;
            try
            {
                hasSuccessfullySaved = appointmentsService.Edit(dbAppointment);
            }
            catch (System.Data.SqlClient.SqlException)
            {
                hasSuccessfullySaved = false;
            }
            if (hasSuccessfullySaved)
            {
                TempData["SuccessfullMessage"] = "Appointment cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "There was a server error while trying to cancel the appointment.";
            }

            return RedirectToAction("ViewAppointments", "Appointments");
        }

        private EditAppointmentViewModel CreateViewModelFromAppointment(Appointment appointment)
        {
            EditAppointmentViewModel viewModel = new EditAppointmentViewModel();
            viewModel.AppointmentId = appointment.AppointmentId;
            viewModel.StartDateTime = appointment.StartDateTime;
            viewModel.EndDateTime = appointment.EndDateTime;
            return viewModel;
        }

        private ActionResult GetRedirectIfAppointmentNotOwned(Appointment appointment)
        {
            if (appointment.UserId != LoginUserSession.Current.UserId)
            {
                TempData["ErrorMessage"] = "Appointments does not exist or you have no rights to edit";
                return RedirectToAction("Index", "Home");
            }

            return null;
        }

        private List<ActivityRow> GetActivityRowsForAllActivities()
        {
            List<ActivityRow> activityRows = new List<ActivityRow>();
            List<Activity> dbActivities = activitiesService.GetAll().ToList();
            foreach (Activity activity in dbActivities)
            {
                ActivityRow activityRow = new ActivityRow();
                activityRow.activity = activity;
                activityRow.isChecked = false;
                activityRows.Add(activityRow);
            }

            return activityRows;
        }

        private void MarkActivityRowsForAppointmentActivitiesAsChecked(List<ActivityRow> activityRows, ICollection<Activity> activities)
        {
            foreach (ActivityRow activityRow in activityRows)
            {
                foreach (Activity activity in activities)
                {
                    if (activityRow.activity.ActivityId == activity.ActivityId)
                    {
                        activityRow.isChecked = true;
                    }
                }
            }
        }
    }
}