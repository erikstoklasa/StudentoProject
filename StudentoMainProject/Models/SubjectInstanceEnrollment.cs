using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class SubjectInstanceEnrollment
    {
        public int Id { get; set; }
        public int StudentGroupId { get; set; }
        public int SubjectInstanceId { get; set; }

        public SubjectInstance SubjectInstance { get; set; }
        public StudentGroup StudentGroup { get; set; }
    }
}
