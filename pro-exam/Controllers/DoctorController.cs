using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pro_exam.DataBaseContext;
using pro_exam.Models;
using pro_exam.ViewModel;
using static pro_exam.ViewModel.DoctorScheduleViewModel;

namespace pro_exam.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDBcontext _context;

        public DoctorController(AppDBcontext context)
        {
            _context = context;
        }




        public ActionResult AddNewDoctor()
        {



            return View();
        }
        [HttpPost]
        public IActionResult AddToDB(pro_exam.Models.Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Doctor has been added successfully!";

            return RedirectToAction("AddNewDoctor");
        }



        // صفحة عرض الأطباء مع المواعيد
        public IActionResult DoctorsWithSchedules()
        {
            var doctors = _context.Doctors
       .Select(doctor => new DoctorScheduleViewModel
       {
           DoctorId = doctor.Id,
           DoctorName = doctor.DoctorName,
           SundayTuesdayThursdaySchedules = doctor.Monitorings
               .Where(m => m.Schedule.Day == "Sunday" || m.Schedule.Day == "Tuesday" || m.Schedule.Day == "Thursday")
               .Select(m => new ScheduleViewModel
               {
                   Day = m.Schedule.Day,
                   StartTime = m.Schedule.StartTime,
                   EndTime = m.Schedule.EndTime
               }).ToList(),
           MondayWednesdaySchedules = doctor.Monitorings
               .Where(m => m.Schedule.Day == "Monday" || m.Schedule.Day == "Wednesday")
               .Select(m => new ScheduleViewModel
               {
                   Day = m.Schedule.Day,
                   StartTime = m.Schedule.StartTime,
                   EndTime = m.Schedule.EndTime
               }).ToList()
       }).ToList();

            return View(doctors);
        }





        public IActionResult EditSundayTuesdayThursday(int doctorId)
        {
            var doctor = _context.Doctors
                .Include(d => d.Monitorings)
                .ThenInclude(m => m.Schedule)
                .FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null)
                return NotFound("Doctor not found");

            var viewModel = new DoctorScheduleViewModel
            {
                DoctorId = doctor.Id,
                DoctorName = doctor.DoctorName,
                SundayTuesdayThursdaySchedules = doctor.Monitorings
                    .Where(m => m.Schedule.Day == "Sunday" || m.Schedule.Day == "Tuesday" || m.Schedule.Day == "Thursday")
                    .Select(m => new ScheduleViewModel
                    {
                        Day = m.Schedule.Day,
                        StartTime = m.Schedule.StartTime,
                        EndTime = m.Schedule.EndTime
                    }).ToList()
            };

            return View(viewModel);
        }


        [HttpPost]
        public IActionResult SaveSundayTuesdayThursday(DoctorScheduleViewModel model)
        {
            var doctor = _context.Doctors
                .Include(d => d.Monitorings)
                .ThenInclude(m => m.Schedule)
                .FirstOrDefault(d => d.Id == model.DoctorId);

            if (doctor == null)
                return NotFound("Doctor not found");

            foreach (var schedule in model.SundayTuesdayThursdaySchedules)
            {
                var scheduleToUpdate = doctor.Monitorings
                    .FirstOrDefault(m => m.Schedule.Day == schedule.Day)?.Schedule;

                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.StartTime = schedule.StartTime;
                    scheduleToUpdate.EndTime = schedule.EndTime;
                }
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Schedules updated successfully!";
            return RedirectToAction("DoctorsWithSchedules");
        }



        public IActionResult EditMondayWednesday(int doctorId)
        {
            var doctor = _context.Doctors
                .Include(d => d.Monitorings)
                .ThenInclude(m => m.Schedule)
                .FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null)
                return NotFound("Doctor not found");

            var viewModel = new DoctorScheduleViewModel
            {
                DoctorId = doctor.Id,
                DoctorName = doctor.DoctorName,
                MondayWednesdaySchedules = doctor.Monitorings
                    .Where(m => m.Schedule.Day == "Monday" || m.Schedule.Day == "Wednesday")
                    .Select(m => new ScheduleViewModel
                    {
                        Day = m.Schedule.Day,
                        StartTime = m.Schedule.StartTime,
                        EndTime = m.Schedule.EndTime
                    }).ToList()
            };

            return View(viewModel);
        }



        [HttpPost]
        public IActionResult SaveMondayWednesday(DoctorScheduleViewModel model)
        {
            var doctor = _context.Doctors
                .Include(d => d.Monitorings)
                .ThenInclude(m => m.Schedule)
                .FirstOrDefault(d => d.Id == model.DoctorId);

            if (doctor == null)
                return NotFound("Doctor not found");

            foreach (var schedule in model.MondayWednesdaySchedules)
            {
                var scheduleToUpdate = doctor.Monitorings
                    .FirstOrDefault(m => m.Schedule.Day == schedule.Day)?.Schedule;

                if (scheduleToUpdate != null)
                {
                    scheduleToUpdate.StartTime = schedule.StartTime;
                    scheduleToUpdate.EndTime = schedule.EndTime;
                }
            }

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Schedules updated successfully!";
            return RedirectToAction("DoctorsWithSchedules");
        }


        [HttpPost]
        public IActionResult DeleteMondayWednesday(int doctorId, string day)
        {
            // البحث عن الطبيب في قاعدة البيانات مع المواعيد
            var doctor = _context.Doctors
                .Include(d => d.Monitorings)
                .ThenInclude(m => m.Schedule)
                .FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            // البحث عن الجدول الخاص باليوم المحدد وحذفه
            var monitoringToDelete = doctor.Monitorings
                .FirstOrDefault(m => m.Schedule.Day == day);

            if (monitoringToDelete != null)
            {
                _context.Montering.Remove(monitoringToDelete);
                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Schedule for {day} deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Schedule not found.";
            }

            return RedirectToAction("DoctorsWithSchedules");
        }



        [HttpPost]
        public IActionResult DeleteSundayTuesdayThursday(int doctorId, string day)
        {
            // البحث عن الطبيب في قاعدة البيانات مع المواعيد
            var doctor = _context.Doctors
                .Include(d => d.Monitorings)
                .ThenInclude(m => m.Schedule)
                .FirstOrDefault(d => d.Id == doctorId);

            if (doctor == null)
            {
                return NotFound("Doctor not found");
            }

            // البحث عن الجدول الخاص باليوم المحدد وحذفه
            var monitoringToDelete = doctor.Monitorings
                .FirstOrDefault(m => m.Schedule.Day == day);

            if (monitoringToDelete != null)
            {
                _context.Montering.Remove(monitoringToDelete);
                _context.SaveChanges();

                TempData["SuccessMessage"] = $"Schedule for {day} deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Schedule not found.";
            }

            return RedirectToAction("DoctorsWithSchedules");
        }












    }
}
