using Microsoft.AspNetCore.Mvc;
using pro_exam.DataBaseContext;
using pro_exam.ViewModel;

namespace pro_exam.Controllers
{
    public class DoctorController : Controller
    {
        private readonly AppDBcontext _context;

        public DoctorController(AppDBcontext context)
        {
            _context = context;
        }

        public ActionResult AddNewDoctor() { 
        
       

        return View();
        }

        [HttpPost]
        public IActionResult AddToDB(pro_exam.Models.Doctor doctor)
        {
            _context.Doctors.Add(doctor);
            _context.SaveChanges();


            return RedirectToAction("GetFreeTime");
        }

            [HttpGet]
        public IActionResult GetFreeTime()
        {
            var model = new DoctorScheduleViewModel
            {
                SundayTuesdayThursdayDoctors = _context.Doctors
                    .Where(d => d.Day == "Sunday" || d.Day == "Tuesday" || d.Day == "Thursday")
                    .OrderBy(d => d.StartTime) // ترتيب تصاعدي حسب StartTime
                    .ToList(),

                MondayWednesdayDoctors = _context.Doctors
                    .Where(d => d.Day == "Monday" || d.Day == "Wednesday")
                    .OrderBy(d => d.StartTime) // ترتيب تصاعدي حسب StartTime
                    .ToList()
            };

            // حساب أوقات الفراغ لكل مجموعة
            ViewBag.SundayTuesdayThursdayFreeTimes = model.GetFreeTimes(model.SundayTuesdayThursdayDoctors);
            ViewBag.MondayWednesdayFreeTimes = model.GetFreeTimes(model.MondayWednesdayDoctors);

            return View(model);
        }


    }
}
