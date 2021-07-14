using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimetableServiceProject.Models
{
    public class Elective
    {
        public int Id { get; set; }

        public List<Student> Students { get; set; }
    }
}
