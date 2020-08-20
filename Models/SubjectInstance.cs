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

        public ICollection<SubjectInstanceEnrollment> Enrollments { get; set; }
        public ICollection<Grade> Grades { get; set; }
        public ICollection<TimeFrame> TimeFrames { set; get; }
        public Teacher Teacher { get; set; }
        public SubjectType SubjectType { get; set; }
    }
}
