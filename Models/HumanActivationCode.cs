using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public enum CodeType { Teacher, Student }
    public class HumanActivationCode
    {
        public int Id { get; set; } 
        public int TargetId { get; set; } //Teacher/Student Id
        public string HumanCode { get; set; }
        public CodeType CodeType { get; set; }
    }

}
