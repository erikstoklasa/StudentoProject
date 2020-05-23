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
        [Display(Name = "Název")]
        [Required(ErrorMessage = "Zadejte prosím název")]
        public string Name { get; set; }
        [Display(Name = "Známka")]
        [Range(1,5, ErrorMessage = "Zadejte známku mezi 1 a 5")]
        public int? Value { get; set; }
        public int StudentId { get; set; } 
        public int SubjectId { get; set; }
        [Display(Name = "Datum přidání")]
        public DateTime Added { get; set; }

        public Student Student { get; set; }
        public Subject Subject { get; set; }
    }
}
