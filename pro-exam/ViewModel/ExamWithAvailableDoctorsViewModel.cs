namespace pro_exam.ViewModel
{ 

    public class ExamWithAvailableDoctorsViewModel
    {
        public int ExamId { get; set; } // معرف الامتحان
        public string CourseName { get; set; } // اسم المادة
        public string Day { get; set; } // يوم الامتحان
        public TimeSpan StartExamTime { get; set; } // وقت بداية الامتحان
        public TimeSpan EndExamTime { get; set; } // وقت نهاية الامتحان
        public List<string> AvailableDoctors { get; set; } // أسماء الدكاترة المتاحين
    }

}
