using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Class : StudentGroup
    {
        //public new int Id { set; get; }
        public short Grade { set; get; } //např. 2 pro 2. A
        public new String Name { set; get; } //např. A pro 2. A
        public int TeacherId { set; get; }

        public Teacher Teacher { set; get; }
        public override string GetName() => $"{Grade}.{Name}";
    }
}
