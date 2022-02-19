using System;

namespace StudentoMainProject.API.Models
{
    public class GradeGroupObject
    {
        public enum USERTYPE { Teacher, Student }
        public int Id { get; set; }
        public int Weight { get; set; }
        public string Name { get; set; }
        public DateTime Added { get; set; }
        public int AddedById { get; set; }
        public USERTYPE AddedBy { get; set; }
    }
}
