using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class AttendanceRecord
    {
        public enum REASON { Absent, Late, Left, AbsentExcused, AbsentExcusedBySchool };

        public int Id { set; get; }
        public int LessonRecordId { set; get; }
        public int StudentId { set; get; }
        public REASON Reason { set; get; }
        public string Description { set; get; }

        public LessonRecord LessonRecord { set; get; }
        public Student Student { set; get; }
    }
}
