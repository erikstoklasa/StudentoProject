using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableServiceProject.Models
{
    public class Student
    {
        public int Id { get; set; }

        public List<Elective> Electives { get; set; }
    }
}
