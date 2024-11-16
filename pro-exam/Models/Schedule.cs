namespace pro_exam.Models
{
    public class Schedule
    {
        public int Id { get; set; } // Primary Key
        public string Day { get; set; } // يوم العمل
        public TimeSpan StartTime { get; set; } // وقت البداية
        public TimeSpan EndTime { get; set; } // وقت النهاية
        public ICollection<Montering> Monitorings { get; set; }

    }
}
