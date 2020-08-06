using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Approbation
    {
        public int Id { get; set; }
        public int TeacherId { get; set; }
        public int SubjectTypeId { get; set; }


        public Teacher Teacher { get; set; }
        public SubjectType SubjectType { get; set; }
    }
}
