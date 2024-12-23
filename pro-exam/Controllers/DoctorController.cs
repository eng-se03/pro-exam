using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pro_exam.DataBaseContext;
using pro_exam.Models;
using pro_exam.ViewModel;
using static pro_exam.ViewModel.DoctorScheduleViewModel;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using ExcelDataReader;

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
            // تحقق إذا كان اسم الدكتور موجودًا بالفعل
            var existingDoctor = _context.Doctors.FirstOrDefault(d => d.DoctorName == doctor.DoctorName);

            if (existingDoctor != null)
            {
                // إضافة رسالة إلى TempData في حالة وجود الدكتور مسبقًا
                TempData["ErrorMessage"] = "Doctor with the same name already exists!";
                return RedirectToAction("AddNewDoctor");
            }

            // إذا لم يكن موجودًا، قم بإضافته إلى قاعدة البيانات
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


        [HttpPost]
        public IActionResult SaveDoctorFreeTime(List<DoctorFreeTime> DoctorFreeTimeList)
        {
            if (DoctorFreeTimeList == null || !DoctorFreeTimeList.Any())
            {
                return RedirectToAction("DoctorsWithSchedules");
            }

            // إضافة البيانات إلى قاعدة البيانات
            _context.DoctorFreeTimes.AddRange(DoctorFreeTimeList);

            // حفظ التغييرات
            _context.SaveChanges();

            return RedirectToAction("DoctorsWithSchedules");
        }







        //  database اضافة الصفحة اكسل على ال 

        [HttpPost]
        public async Task<IActionResult> ExcelFileReader(IFormFile file)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";
                if (!Directory.Exists(uploadDirectory))
                {
                    Directory.CreateDirectory(uploadDirectory);
                }

                var filePath = Path.Combine(uploadDirectory, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // قراءة الملف وتخزين البيانات في قاعدة البيانات
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        while (reader.Read())
                        {
                            // تخطي الصف الأول إذا كان يحتوي على رؤوس الأعمدة
                            if (reader.Depth == 0) continue;

                            // قراءة البيانات من كل عمود
                            var doctorName = reader.GetValue(0)?.ToString();
                            var day = reader.GetValue(1)?.ToString();
                            var startTime = TimeSpan.Parse(reader.GetValue(2)?.ToString());
                            var endTime = TimeSpan.Parse(reader.GetValue(3)?.ToString());

                            // التحقق من صحة البيانات
                            if (string.IsNullOrEmpty(doctorName) || string.IsNullOrEmpty(day))
                                continue;

                            // حفظ البيانات في قاعدة البيانات
                            await SaveRecordToDatabase(doctorName, day, startTime, endTime);
                        }
                    }
                }
            }

            return RedirectToAction("DoctorsWithSchedules"); // أو عرض رسالة نجاح
        }

        private async Task SaveRecordToDatabase(string doctorName, string day, TimeSpan startTime, TimeSpan endTime)
        {
            // التحقق من وجود الطبيب
            var doctor = await _context.Doctors.FirstOrDefaultAsync(d => d.DoctorName == doctorName);
            if (doctor == null)
            {
                doctor = new Doctor
                {
                    DoctorName = doctorName
                };
                _context.Doctors.Add(doctor);
                await _context.SaveChangesAsync();
            }

            // إنشاء جدول زمني جديد
            var schedule = new Schedule
            {
                Day = day,
                StartTime = startTime,
                EndTime = endTime
            };
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();

            // ربط الطبيب بالجدول الزمني
            var monitoring = new Montering
            {
                DoctorId = doctor.Id,
                ScheduleId = schedule.Id
            };
            _context.Montering.Add(monitoring);
            await _context.SaveChangesAsync();
        }


    }
}









