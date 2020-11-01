using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class LessonRecord
    {
        public int Id { set; get; }
        public int? TimeFrameId { set; get; }
        public int? SubjectInstanceId { get; set; }
        public int? Week { get; set; }
        public string? Description { set; get; }
        public string? SafetyInstructions { set; get; }


        public TimeFrame? TimeFrame { set; get; }
        public SubjectInstance? SubjectInstance { get; set; }
        public ICollection<AttendanceRecord>? Attendance { set; get; }
    }
}
