

namespace pro_exam.Models
{
    public class Doctor
    {

        public int Id { get; set; }
        public string DoctorName { get; set; }
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }


    }
}