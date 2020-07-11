using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public int SubjectInstanceId { get; set; }

        public SubjectInstance SubjectInstance { get; set; }
        public Student Student { get; set; }
    }
}
