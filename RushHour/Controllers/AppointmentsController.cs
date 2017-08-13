using DataAccess.Models;
using Repository;
using RushHour.Helpers;
using RushHour.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RushHour.Controllers
{
    public class AppointmentsController : Controller
    {
        UnitOfWork unitOfWork = new UnitOfWork();

        public ActionResult AddAppointment()
        {
            IEnumerable<Activity> dbActivities = unitOfWork.ActivitiesRepository.GetAll();

            List<ActivityRow> activityRows = new List<ActivityRow>();
            AddAppointmentViewModel viewModel = new AddAppointmentViewModel();
            DateTime currentTime = DateTime.Now;
            viewModel.StartDateTime = currentTime;//String.Concat(currentTime.Year, "-", String.Format("{0:00}", currentTime.Month), "-", String.Format("{0:00}", currentTime.Day), "T", String.Format("{0:00}", currentTime.Hour), ":",
                //String.Format("{0:00}", currentTime.Minute)/*, ":", String.Format("{0:00}", currentTime.Second)*/);

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
        public ActionResult AddAppointment(AddAppointmentViewModel viewModel)
        {
            Appointment appointment = new Appointment();

            double summedMinutes = 0;
            List<Activity> activities = unitOfWork.ActivitiesRepository.GetAll(a => viewModel.CheckedRows.Contains(a.ActivityId)).ToList();
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
            unitOfWork.AppointmentsRepository.Add(appointment);
            bool hasSavedSuccessfully = unitOfWork.Save();
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

        public ActionResult ViewAppointments()
        {
            List<Appointment> appointments = unitOfWork.AppointmentsRepository.GetAll(a => a.UserId == LoginUserSession.Current.UserId).ToList();
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

        public ActionResult EditAppointment(int id)
        {
            Appointment dbAppointment = unitOfWork.AppointmentsRepository.GetById(id);
            if (dbAppointment.UserId != LoginUserSession.Current.UserId)
            {
                TempData["ErrorMessage"] = "Appointments does not exist or you have no rights to edit";
                return RedirectToAction("Index", "Home");
            }

            List<ActivityRow> activityRows = new List<ActivityRow>();
            EditAppointmentViewModel viewModel = new EditAppointmentViewModel();
            viewModel.AppointmentId = dbAppointment.AppointmentId;
            viewModel.StartDateTime = dbAppointment.StartDateTime;
            viewModel.EndDateTime = dbAppointment.EndDateTime;
            List<Activity> dbActivities = unitOfWork.ActivitiesRepository.GetAll().ToList();
            foreach (Activity activity in dbActivities)
            {
                ActivityRow activityRow = new ActivityRow();
                activityRow.activity = activity;
                activityRow.isChecked = false;
                activityRows.Add(activityRow);
            }

            foreach (ActivityRow activityRow in activityRows)
            {
                foreach (Activity activity in dbAppointment.Activities)
                {
                    if (activityRow.activity.ActivityId == activity.ActivityId)
                    {
                        activityRow.isChecked = true;
                    }
                }
            }
            viewModel.activityRows = activityRows;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointment(EditAppointmentViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Appointment dbAppointment = unitOfWork.AppointmentsRepository.GetById(viewModel.AppointmentId);
                dbAppointment.StartDateTime = viewModel.StartDateTime;
                double summedMinutes = 0;
                dbAppointment.Activities.Clear();
                List<Activity> activities = unitOfWork.ActivitiesRepository.GetAll(a => viewModel.CheckedRows.Contains(a.ActivityId)).ToList();
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
                unitOfWork.AppointmentsRepository.Edit(dbAppointment);
                bool hasSavedSuccessfully = unitOfWork.Save();
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

        public ActionResult CancelAppointment(int id)
        {
            Appointment dbAppointment = unitOfWork.AppointmentsRepository.GetById(id);
            dbAppointment.IsCancelled = true;
            unitOfWork.AppointmentsRepository.Edit(dbAppointment);
            bool hasSuccessfullySaved = true;
            try
            {
                hasSuccessfullySaved = unitOfWork.Save();
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
    }
}