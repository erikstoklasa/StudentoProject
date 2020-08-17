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
        public bool IsPrivate { set; get; }
        public int TeacherId { set; get; }
        public Teacher Teacher { set; get; }

        public ICollection<Student> Students { set; get; }
        public ICollection<Enrollment> Enrollments { set; get; }

        public virtual string GetName() => Name;
    }
}
