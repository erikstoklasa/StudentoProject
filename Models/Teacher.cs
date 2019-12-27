using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string? UserAuthId { get; set; }
        [Display(Name = "Jméno")]
        public String FirstName { get; set; }
        [Display(Name = "Přijímení")]
        public String LastName { get; set; }
        public ICollection<Subject> Subjects { get; set; }

    }
}
