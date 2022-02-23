using System.Linq;

namespace StudentoMainProject.API.Models
{
    public class UserObject
    {
        #nullable enable
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public USERTYPE UserType { get; set; }
        public string? Birthdate { get; set; }
        public string? PersonalIdentifNumber { get; set; }
        public string? PlaceOfBirth { get; set; }
        public string? IdentifCardNumber { get; set; }
        public string? InsuranceCompany { get; set; }
        //Address
        public string? StreetAddress { get; set; }
        public string? CityAddress { get; set; }
        public string? ZipCode { get; set; }
        //Contact
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? SchoolId { get; set; }
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
        public enum USERTYPE { Teacher, Student, Admin, Parent, TeacherClassmaster }
    }
}
