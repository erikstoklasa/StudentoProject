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
        public string? FirstName { get; set; }
        [Display(Name = "Příjmení")]
        public string? LastName { get; set; }
        [Display(Name = "Datum narození")]
        public DateTime? Birthdate { get; set; }
        [Display(Name = "Rodné číslo")]
        public string? PersonalIdentifNumber { get; set; }
        [Display(Name = "Místo narození")]
        public string? PlaceOfBirth { get; set; }
        [Display(Name = "Datum nástupu")]
        public DateTime? StartDate { get; set; }
        [Display(Name = "Pojišťovna")]
        public string? InsuranceCompany { get; set; }
        [Display(Name = "Číslo občanského průkazu")]
        public string? IdentifCardNumber { get; set; }
        [Display(Name = "Dosažené vzdělání")]
        public string? EducationLevel { get; set; }
        //Address
        [Display(Name = "Ulice a čp.")]
        public string? StreetAddress { get; set; }
        [Display(Name = "Město")]
        public string? CityAddress { get; set; }
        [Display(Name = "PSČ")]
        public string? ZipCode { get; set; }
        //Contact
        public string? Email { get; set; }
        [Display(Name = "Telefonní číslo")]
        public string? PhoneNumber { get; set; }
        public int SchoolId { get; set; }
        public string GetFullName()
        {
            FirstName ??= "";
            LastName ??= "";
            return $"{FirstName} {LastName}";
        }

        public string GetInitials()
        {
            FirstName ??= "";
            LastName ??= "";
            return $"{FirstName.ToUpper().First()}{LastName.ToUpper().First()}";
        }

        public ICollection<SubjectInstance>? SubjectInstances { get; set; }
        public ICollection<Approbation>? Approbations { set; get; }
        public ICollection<SubjectMaterial>? SubjectMaterials { set; get; }
        public School? School { get; set; }
    }
}
