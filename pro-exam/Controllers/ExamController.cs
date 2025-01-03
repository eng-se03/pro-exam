﻿using ExcelDataReader;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pro_exam.DataBaseContext;
using pro_exam.Models;
using pro_exam.ViewModel;
using OfficeOpenXml;


namespace pro_exam.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
                            var CourseName = reader.GetValue(0)?.ToString();
                            var Day = reader.GetValue(1)?.ToString();
                            var StartExamTime = TimeSpan.Parse(reader.GetValue(2)?.ToString());
                            var EndExamTime = TimeSpan.Parse(reader.GetValue(3)?.ToString());

                            // التحقق من صحة البيانات
                            if (string.IsNullOrEmpty(CourseName) || string.IsNullOrEmpty(Day))
                                continue;

                            // حفظ البيانات في قاعدة البيانات
                            await SaveRecordToDatabase(CourseName, Day, StartExamTime, EndExamTime);
                        }
                    }
                }
            }

            return RedirectToAction("ExamDashBoard"); // أو عرض رسالة نجاح
        }

        private async Task SaveRecordToDatabase(string CourseName, string Day, TimeSpan StartExamTime, TimeSpan EndExamTime)
        {


            // إنشاء جدول زمني جديد
            var exam = new Exam
            {
                CourseName = CourseName,
                Day = Day,
                StartExamTime = StartExamTime,
                EndExamTime = EndExamTime
            };
            _context.Exams.Add(exam);
            await _context.SaveChangesAsync();

        }


        [HttpPost]
        public IActionResult ResetDatabase()
        {
            try
            {
                // حذف جميع البيانات من الجداول            
                _context.Exams.RemoveRange(_context.Exams);
                _context.DoctorFreeTimes.RemoveRange(_context.DoctorFreeTimes);

                // حفظ التغييرات
                _context.SaveChanges();

                // إضافة رسالة نجاح
                TempData["SuccessMessage"] = "Database has been reset successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"An error occurred while resetting the database: {ex.Message}";
            }

            return RedirectToAction("Index"); // استبدل بـ الصفحة المناسبة
        }






        [HttpGet]
        public IActionResult GenerateExamReport()
        {
            var examsWithDoctors = _context.Exams
                .Select(exam => new ExamWithAvailableDoctorsViewModel
                {
                    ExamId = exam.Id,
                    CourseName = exam.CourseName,
                    Day = exam.Day,
                    StartExamTime = exam.StartExamTime,
                    EndExamTime = exam.EndExamTime,
                    AvailableDoctors = _context.Doctors
                        .Where(doctor => doctor.Monitorings.Any(m =>
                            m.Schedule.Day == exam.Day &&
                            m.Schedule.StartTime <= exam.StartExamTime &&
                            m.Schedule.EndTime >= exam.EndExamTime))
                        .Select(d => d.DoctorName)
                        .ToList()
                }).ToList();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Exam Report");

                // Add Headers
                worksheet.Cells[1, 1].Value = "Exam ID";
                worksheet.Cells[1, 2].Value = "Course Name";
                worksheet.Cells[1, 3].Value = "Day";
                worksheet.Cells[1, 4].Value = "Start Time";
                worksheet.Cells[1, 5].Value = "End Time";
                worksheet.Cells[1, 6].Value = "Available Doctors";

                // Add Data
                int row = 2;
                foreach (var exam in examsWithDoctors)
                {
                    worksheet.Cells[row, 1].Value = exam.ExamId;
                    worksheet.Cells[row, 2].Value = exam.CourseName;
                    worksheet.Cells[row, 3].Value = exam.Day;
                    worksheet.Cells[row, 4].Value = exam.StartExamTime.ToString(@"hh\:mm");
                    worksheet.Cells[row, 5].Value = exam.EndExamTime.ToString(@"hh\:mm");
                    worksheet.Cells[row, 6].Value = string.Join(", ", _context.DoctorFreeTimes
                        .Where(freeTime =>
                            freeTime.Day == exam.Day && // مقارنة يوم الامتحان مع يوم الفراغ
                            freeTime.StartFreeTime <= exam.StartExamTime && // وقت بداية الفراغ يغطي بداية الامتحان
                            freeTime.EndFreeTime >= exam.EndExamTime) // وقت نهاية الفراغ يغطي نهاية الامتحان
                        .Select(ft => ft.DoctorName) // جلب أسماء الدكاترة المتاحين
                        .ToList());

                    row++;
                }

                // Auto-fit columns
                worksheet.Cells.AutoFitColumns();

                // Generate File
                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string fileName = "Exam_Report.xlsx";
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(stream, contentType, fileName);
            }


        }
        }
}
