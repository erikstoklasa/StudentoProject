using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string? UserAuthId { get; set; }
        [Display(Name="Jméno")]
        public String FirstName { get; set; }
        [Display(Name = "Přijímení")]
        public String LastName { get; set; }

        public String getFullName() => FirstName + " " + LastName;

        public ICollection<Enrollment> Enrollments { get; set; }
        public ICollection<Grade> Grades { get; set; }
    }
}
