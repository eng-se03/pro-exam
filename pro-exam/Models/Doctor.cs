

using System.ComponentModel.DataAnnotations;

namespace pro_exam.Models
{
    public class Doctor
    {
       
        public int Id { get; set; } // Primary Key
        public string DoctorName { get; set; } // اسم الدكتور
        public ICollection<Montering> Monitorings { get; set; }

    }
}