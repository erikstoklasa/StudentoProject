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

        public TimetableRecord? TimetableRecord { get; set; } //Possible attachment
    }
}
