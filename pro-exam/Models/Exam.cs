namespace pro_exam.Models
{
    public class Exam
    {
        public int Id { get; set; }

        public string CourseName { get; set; }
        public TimeSpan StartExamTime { get; set; }
        public TimeSpan EndExamTime { get; set; }

    }
}
