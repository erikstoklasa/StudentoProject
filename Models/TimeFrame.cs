using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class TimeFrame
    {
        public int Id { set; get; }
        public int? SubjectInstanceId { set; get; }
        public DayOfWeek DayOfWeek { set; get; }
        public DateTime Start { set; get; }
        public DateTime End { set; get; }
        public ushort Reccurance { set; get; }
        public int? RoomId { set; get; }

        public SubjectInstance SubjectInstance { set; get; }
        public Room Room { set; get; }
    }
}
