using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class GradeAverage
    {
        public enum USERTYPE { Teacher, Student }

        public int Id { get; set; }
        public int SubjectInstanceId { get; set; }
        public double Value { get; private set; } //For table in the db
        public int TeacherId { get; set; }
        public int StudentId { get; set; }
        public DateTime Added { get; set; }

    }
}
