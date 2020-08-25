using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class TimetableRecord
    {
        public int Id { set; get; }
        public int? SubjectInstanceId { set; get; }
        public ushort Reccurance { set; get; }
        public int? RoomId { set; get; }
        public int? TimeFrameId { set; get; }

        public SubjectInstance SubjectInstance { set; get; }
        public TimeFrame TimeFrame { set; get; }
        public Room Room { set; get; }
    }
}
