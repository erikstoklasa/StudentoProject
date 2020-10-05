using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class TimetableChange
    {
        public int Id { get; set; }
        public int TimeFrameId { set; get; }
        public int? StudentGroupId { get; set; }
        public int? SubjectInstanceId { get; set; }
        public int Week { get; set; }

        public bool? Canceled { set; get; }
        public int? CurrentSubjectInstanceId { set; get; }
        public int? CurrentTeacherId { set; get; }
        public int? CurrentRoomId { set; get; }

        public StudentGroup StudentGroup { set; get; }
        public SubjectInstance CurrentSubjectInstance { set; get; }
        public Teacher CurrentTeacher { set; get; }
        public Room CurrentRoom { set; get; }
        public TimeFrame TimeFrame { set; get; }
    }
}
