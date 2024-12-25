using Microsoft.AspNetCore.Mvc;
using pro_exam.DataBaseContext;
using pro_exam.Models;

public class SchdualController : Controller
{
    private readonly AppDBcontext _context;

    public SchdualController(AppDBcontext context)
    {
        _context = context;
    }

    // عرض صفحة إنشاء جدول جديد
    public IActionResult CreateSchedule()
    {
        // إحضار قائمة الأطباء لعرضها في الـ Dropdown
        ViewBag.Doctors = _context.Doctors.ToList();
        return View();
    }

    [HttpPost]
    public IActionResult CreateSchedule(Schedule schedule, int doctorId)
    {
        
            // حفظ جدول الأوقات
            _context.Schedules.Add(schedule);
            _context.SaveChanges();

            // إنشاء الربط بين الطبيب والجدول
            var monitoring = new Montering
            {
                DoctorId = doctorId,
                ScheduleId = schedule.Id
            };
            _context.Montering.Add(monitoring);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Schedule has been created and linked to the doctor successfully!";
            return RedirectToAction("CreateSchedule");
        

    }



}
