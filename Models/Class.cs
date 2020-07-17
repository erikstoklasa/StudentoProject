using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Class
    {
        public int Id { set; get; }
        public short Grade { set; get; } //např. 2 pro 2. A
        public String Name { set; get; } //např. A pro 2. A

        public ICollection<Student> Students { set; get; }
    }
}
