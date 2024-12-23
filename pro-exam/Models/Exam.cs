namespace pro_exam.Models
{
    public class Exam
    {
        public int Id { get; set; }
        
        public string CourseName { get; set; }
        public string Day { get; set; } // يوم العمل
        public TimeSpan StartExamTime { get; set; }
        public TimeSpan EndExamTime { get; set; }

        //public int DoctorId { get; set; }
    }
}
