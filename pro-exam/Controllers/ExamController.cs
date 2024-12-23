using Microsoft.AspNetCore.Mvc;
using pro_exam.DataBaseContext;
using pro_exam.Models;
using pro_exam.ViewModel;

namespace pro_exam.Controllers
{
    public class ExamController : Controller
    {
        private readonly AppDBcontext _context;

        public ExamController(AppDBcontext context)
        {
            _context = context;
        }
        // عرض قائمة جميع الامتحانات
        public IActionResult ExamDashBoard()
        {
            var examsWithDoctors = _context.Exams
                .Select(exam => new ExamWithAvailableDoctorsViewModel
                {
                    ExamId = exam.Id,
                    CourseName = exam.CourseName,
                    Day = exam.Day,
                    StartExamTime = exam.StartExamTime,
                    EndExamTime = exam.EndExamTime,
                    AvailableDoctors = _context.DoctorFreeTimes
                        .Where(freeTime =>
                            freeTime.Day == exam.Day && // مقارنة يوم الامتحان مع يوم الفراغ
                            freeTime.StartFreeTime <= exam.StartExamTime && // وقت بداية الفراغ يغطي بداية الامتحان
                            freeTime.EndFreeTime >= exam.EndExamTime) // وقت نهاية الفراغ يغطي نهاية الامتحان
                        .Select(ft => ft.DoctorName) // جلب أسماء الدكاترة المتاحين
                        .ToList()
                }).ToList();

            return View(examsWithDoctors);
        }













        public IActionResult AddExam()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddExam(Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Exams.Add(exam);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Exam added successfully!";
                return RedirectToAction("ExamDashBoard");
            }
            return View(exam);
        }

        public IActionResult EditExam(int id)
        {
            var exam = _context.Exams.Find(id);
            if (exam == null) return NotFound();
            return View(exam);
        }

        [HttpPost]
        public IActionResult SaveEditExam(Exam exam)
        {
            if (ModelState.IsValid)
            {
                _context.Exams.Update(exam);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Exam updated successfully!";
                return RedirectToAction("ExamDashBoard");
            }
            return View(exam);
        }

        public IActionResult DeleteExam(int id)
        {
            var exam = _context.Exams.Find(id);
            if (exam == null) return NotFound();
            _context.Exams.Remove(exam);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Exam deleted successfully!";
            return RedirectToAction("ExamDashBoard");
        }
    }
}
