using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class LessonRecord
    {
        public int Id { set; get; }
        public int TimeFrameId { set; get; }
        public string Description { set; get; }
        public bool Canceled { set; get; }
        public int? SubstitutionSubjectInstanceId { set; get; }
        public int? SubstitutionTeacherId { set; get; }

        public SubjectInstance SubstitutionSubjectInstance { set; get; }
        public Teacher SubstitutionTeacher { set; get; }
        public ICollection<AttendanceRecord> Attendance { set; get; }
    }
}
