using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Grade
    {
        public int Id { get; set; }
        [Display(Name = "Známka")]
        public int? Value { get; set; }
        public int StudentId { get; set; } 
        public int SubjectId { get; set; }
        [Display(Name = "Datum přidání")]
        public DateTime Added { get; set; }

        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
