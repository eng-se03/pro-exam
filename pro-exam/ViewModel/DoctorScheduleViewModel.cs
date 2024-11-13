using System.Collections.Generic;
using pro_exam.Models;

namespace pro_exam.ViewModel
{
    public class DoctorScheduleViewModel
    {
        public List<Doctor> SundayTuesdayThursdayDoctors { get; set; }
        public List<Doctor> MondayWednesdayDoctors { get; set; }

        public List<string> GetFreeTimes(List<Doctor> doctors)

        {
            var freeTimes = new List<string>();

            // تأكد من ترتيب الفترات الزمنية
            var sortedDoctors = doctors.OrderBy(d => d.StartTime).ToList();

            for (int i = 0; i < sortedDoctors.Count - 1; i++)
            {
                var endCurrent = sortedDoctors[i].EndTime;
                var startNext = sortedDoctors[i + 1].StartTime;

                // إذا كان هناك فترة فارغة بين نهاية الفترة الحالية وبداية الفترة التالية
                if (endCurrent < startNext)
                {
                    freeTimes.Add($"من {endCurrent:hh\\:mm} إلى {startNext:hh\\:mm}");
                }
            }

            return freeTimes;
        }


        public int DoctorId { get; set; }
        public string DoctorName { get; set; }

        // قائمة تحتوي على أوقات العمل للطبيب
        public List<DoctorSchedule> DoctorSchedules { get; set; }
    }

    public class DoctorSchedule
    {
        public int ScheduleId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
