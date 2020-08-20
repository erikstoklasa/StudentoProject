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
        public string Name { set; get; } //např. A pro 2. A
        public int TeacherId { set; get; }
        public int BaseRoomId { set; get; }

        public Teacher Teacher { set; get; }
        public string GetName() => $"{Grade}.{Name}";
        public ICollection<StudentGroupEnrollment> StudentGroupEnrollments { get; set; }
        public Room BaseRoom { set; get; }
        public ICollection<Student> Students { set; get; }
    }
}
