namespace pro_exam.Models
{
    public class DoctorFreeTime
    {
        
            public int Id { get; set; } // Primary Key
            public int DoctorId { get; set; } // Foreign Key
            public string DoctorName { get; set; }
            public Doctor Doctor { get; set; } // Navigation Property
            public string Day { get; set; } // يوم العمل
            public TimeSpan StartFreeTime { get; set; } // بداية وقت الفراغ
            public TimeSpan EndFreeTime { get; set; } // نهاية وقت الفراغ
        
    }
}
