using Microsoft.AspNetCore.Mvc;
using pro_exam.DataBaseContext;
using pro_exam.Models;

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
            var exams = _context.Exams.ToList();
            return View(exams);
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
