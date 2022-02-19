using System;

namespace StudentoMainProject.API.Models
{
    public class GradeObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int? Weight { get; set; }
        public int SubjectInstanceId { get; set; }
        public int StudentId { get; set; }
        public int? GradeGroupId { get; set; }
        public string GradeGroupName { get; set; }
        public DateTime? GradeGroupAdded { get; set; }
        public int? GradeGroupWeight { get; set; }
        public DateTime Added { get; set; }
#nullable enable
        public UserObject? AddedBy { get; set; }
#nullable disable

    }
}
