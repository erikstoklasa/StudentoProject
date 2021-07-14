using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Room
    {
        public int Id { set; get; }
        public string Name { set; get; }
        public int SchoolId { get; set; }

        public ICollection<TimeFrame> TimeFrames { set; get; }
        public School School { get; set; }
    }
}
