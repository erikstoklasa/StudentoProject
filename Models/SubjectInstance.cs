using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class SubjectInstance
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GetName()
        {
            SubjectType.Name ??= "";
            if(Teacher != null)
            {
                return $"{SubjectType.Name} - {Teacher.GetInitials()}";
            }
            return $"{SubjectType.Name}";
        }
        public string GetFullName()
        {
            SubjectType.Name ??= "";
            if (Teacher != null)
            {
                return $"{SubjectType.Name} - {Teacher.GetFullName()}";
            }
            return $"{SubjectType.Name}";
        }

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public Teacher Teacher { get; set; }
        public SubjectType SubjectType { get; set; }
    }
}
