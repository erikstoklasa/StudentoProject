using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Teacher
    {
        #nullable enable
        public int Id { get; set; }
        public string? UserAuthId { get; set; }
        [Display(Name = "Jméno")]
        public String? FirstName { get; set; }
        [Display(Name = "Příjmení")]
        public String? LastName { get; set; }
        public DateTime? Birthdate { get; set; }
        public String? PersonalIdentifNumber { get; set; }
        //Address
        public String? StreetAdrress { get; set; }
        public String? CityAdrress { get; set; }
        public String? ZipCode { get; set; }
        //Contact
        public String? Email { get; set; }
        public String? PhoneNumber { get; set; }
        public String getFullName() => FirstName + " " + LastName;

        public ICollection<Subject>? Subjects { get; set; }

    }
}
