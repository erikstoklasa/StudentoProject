using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Parent
    {
        #nullable enable
        public int Id { get; set; }
        public string? UserAuthId { get; set; }
        
        public ICollection<Student>? Students { set; get; }
    }
}
