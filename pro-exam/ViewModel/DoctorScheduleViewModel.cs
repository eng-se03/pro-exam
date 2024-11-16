using System;
using System.Collections.Generic;

namespace pro_exam.ViewModel
{
    public class DoctorScheduleViewModel
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public List<ScheduleViewModel> SundayTuesdayThursdaySchedules { get; set; }
        public List<ScheduleViewModel> MondayWednesdaySchedules { get; set; }
    }

    public class ScheduleViewModel
    {
        public string Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }
}

