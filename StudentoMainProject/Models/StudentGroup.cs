using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class StudentGroup
    {
        public int Id { set; get; }
        public String Name { set; get; }
        public int? ClassId { set; get; }
        public int SchoolId { get; set; }

        public ICollection<StudentGroupEnrollment> StudentGroupEnrollments { set; get; }
        public ICollection<SubjectInstanceEnrollment> Enrollments { set; get; }
        public Class Class { set; get; }
        public School School { get; set; }

        public virtual string GetName() => Name;
    }
}
