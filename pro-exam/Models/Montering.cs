using System.Numerics;

namespace pro_exam.Models
{
    public class Montering
    {
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public int ScheduleId { get; set; }
        public Doctor Doctors { get; set; }
        public Schedule Schedule { get; set; }


    }
}
