using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public String Name { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public Teacher Teacher { get; set; }
    }
}
