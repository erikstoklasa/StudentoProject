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
        [Display(Name = "Datum narození")]
        public DateTime? Birthdate { get; set; }
        [Display(Name = "Rodné číslo")]
        public String? PersonalIdentifNumber { get; set; }
        //Address
        [Display(Name = "Ulice a čp.")]
        public String? StreetAddress { get; set; }
        [Display(Name = "Město")]
        public String? CityAddress { get; set; }
        [Display(Name = "PSČ")]
        public String? ZipCode { get; set; }
        //Contact
        public String? Email { get; set; }
        [Display(Name = "Telefonní číslo")]
        public String? PhoneNumber { get; set; }
        public String getFullName() => FirstName + " " + LastName;

        public ICollection<SubjectInstance>? SubjectInstances { get; set; }
        public ICollection<SubjectMaterial>? SubjectMaterials { set; get; }
    }
}
