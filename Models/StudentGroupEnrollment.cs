using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class StudentGroupEnrollment
    {
        public int Id { get; set; }
        public int StudentGroupId { get; set; }
        public int StudentId { get; set; }

        public Student Student { get; set; }
        public StudentGroup StudentGroup { get; set; }
    }
}
