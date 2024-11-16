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
            var doctorsWithSchedules = _context.Doctors
     .Include(d => d.Monitorings)
     .ThenInclude(m => m.Schedule)
     .Select(d => new
     {
         DoctorId = d.Id,
         DoctorName = d.DoctorName,
         Schedules = d.Monitorings.Select(m => m.Schedule).ToList()
     })
     .ToList();


            return View(doctorsWithSchedules);
        }


        [HttpGet]
        public IActionResult EditSchedule(int Id)
        {
            // البحث عن الجدول باستخدام Id
            var schedule = _context.Schedules.Find(Id);
            if (schedule == null)
            {
                return NotFound();
            }
            return View(schedule); // عرض صفحة التعديل مع البيانات الموجودة
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditScheduleToDB(int Id, Schedule updatedSchedule)
        {
            if (Id != updatedSchedule.Id)
            {
                return BadRequest();
            }

                // البحث عن الجدول المراد تعديله
                var schedule = _context.Schedules.Find(Id);
                if (schedule == null)
                {
                    return NotFound();
                }

                // تحديث بيانات الجدول
                schedule.Day = updatedSchedule.Day;
                schedule.StartTime = updatedSchedule.StartTime;
                schedule.EndTime = updatedSchedule.EndTime;

                // حفظ التعديلات
                _context.SaveChanges();
                return RedirectToAction("DoctorsWithSchedules"); // إعادة التوجيه إلى صفحة العرض
            

            return View(updatedSchedule); // إذا لم يكن النموذج صحيحًا، العودة إلى صفحة التعديل مع البيانات
        }
        [HttpPost]
        public IActionResult DeleteSchedule(int Id)
        {
            // البحث عن الجدول باستخدام Id
            var schedule = _context.Schedules.Find(Id);
            if (schedule == null)
            {
                return NotFound();
            }

            // حذف الجدول
            _context.Schedules.Remove(schedule);
            _context.SaveChanges();

            // بعد الحذف، العودة إلى صفحة العرض الرئيسية
            return RedirectToAction("DoctorsWithSchedules"); // قم بتغيير "DoctorsWithSchedules" حسب اسم صفحة العرض لديك
        }






























        //public ActionResult AddNewDoctor() { 



        //return View();
        //}

        //[HttpPost]
        //public IActionResult AddToDB(pro_exam.Models.Doctor doctor)
        //{
        //    _context.Doctors.Add(doctor);
        //    _context.SaveChanges();


        //    return RedirectToAction("GetFreeTime");
        //}

        //    [HttpGet]
        //public IActionResult GetFreeTime()
        //{
        //    var model = new DoctorScheduleViewModel
        //    {
        //        SundayTuesdayThursdayDoctors = _context.Doctors
        //            .Where(d => d.Day == "Sunday" || d.Day == "Tuesday" || d.Day == "Thursday")
        //            .OrderBy(d => d.StartTime) // ترتيب تصاعدي حسب StartTime
        //            .ToList(),

        //        MondayWednesdayDoctors = _context.Doctors
        //            .Where(d => d.Day == "Monday" || d.Day == "Wednesday")
        //            .OrderBy(d => d.StartTime) // ترتيب تصاعدي حسب StartTime
        //            .ToList()
        //    };

        //    // حساب أوقات الفراغ لكل مجموعة
        //    ViewBag.SundayTuesdayThursdayFreeTimes = model.GetFreeTimes(model.SundayTuesdayThursdayDoctors);
        //    ViewBag.MondayWednesdayFreeTimes = model.GetFreeTimes(model.MondayWednesdayDoctors);

        //    return View(model);
        //}

        //[HttpGet]
        //public IActionResult EditDoctor(int Id)
        //{
        //    var doctor = _context.Doctors.Find(Id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(doctor);
        //}
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult EditToDB(int Id, Doctor updatedDoctor)
        //{
        //    if (Id != updatedDoctor.Id)
        //    {
        //        return BadRequest();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        var doctor = _context.Doctors.Find(Id);
        //        if (doctor == null)
        //        {
        //            return NotFound();
        //        }

        //        // تحديث بيانات الدكتور
        //        doctor.DoctorName = updatedDoctor.DoctorName;
        //        doctor.StartTime = updatedDoctor.StartTime;
        //        doctor.EndTime = updatedDoctor.EndTime;
        //        doctor.Day = updatedDoctor.Day;

        //        // حفظ التعديلات
        //        _context.SaveChanges();
        //        return RedirectToAction("GetFreeTime");
        //    }

        //    return View(updatedDoctor);
        //}
        //[HttpPost]
        //public IActionResult DeleteDoctor(int Id)
        //{
        //    var doctor = _context.Doctors.Find(Id);
        //    if (doctor == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Doctors.Remove(doctor);
        //    _context.SaveChanges();

        //    // بعد الحذف، العودة إلى الصفحة السابقة أو الصفحة الرئيسية
        //    return RedirectToAction("GetFreeTime");
        //}




    }
}
