using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Student
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
        [Display(Name = "Místo narození")]
        public String? PlaceOfBirth { get; set; }
        [Display(Name = "Číslo občanského průkazu")]
        public String? IdentifCardNumber { get; set; }
        [Display(Name = "Pojišťovna")]
        public String? InsuranceCompany { get; set; }
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

        public int? ClassId { set; get; }
        public int? ParentId { set; get; }

        public String GetFullName()
        {
            FirstName ??= "";
            LastName ??= "";
            return $"{FirstName} {LastName}";
        }

        public String GetInitials()
        {
            FirstName ??= "";
            LastName ??= "";
            return $"{FirstName.ToUpper().First()}{LastName.ToUpper().First()}";
        }

        public ICollection<Enrollment>? Enrollments { get; set; }
        public ICollection<Grade>? Grades { get; set; }
    }
}
