using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
#nullable enable
    public class TimeFrame
    {
        public int Id { set; get; }
        public DayOfWeek DayOfWeek { set; get; }
        public DateTime Start { set; get; }
        public DateTime End { set; get; }
        public int SchoolId { get; set; }

        public TimetableRecord? TimetableRecord { get; set; } //Possible attachment
        public LessonRecord? LessonRecord { get; set; } //Possible attachment
        public TimetableChange? TimetableChange { get; set; } //Possible attachment
        public School? School { get; set; }
    }
}
