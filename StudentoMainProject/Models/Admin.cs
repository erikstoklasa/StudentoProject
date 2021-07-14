using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class Admin
    {
#nullable enable
        public int Id { get; set; }
        public string? UserAuthId { get; set; }
        public int SchoolId { get; set; }
        public int AdminLevel { set; get; }
        public School? School { get; set; }
    }
}
