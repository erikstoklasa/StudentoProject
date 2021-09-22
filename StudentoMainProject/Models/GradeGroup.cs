using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class GradeGroup
    {
        public enum USERTYPE { Teacher, Student }
        public int Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public DateTime Added { get; set; }
        public USERTYPE AddedBy { get; set; }
        public int  AddedById { get; set; }

        public ICollection<Grade> Grades { get; set; }
    }
}
