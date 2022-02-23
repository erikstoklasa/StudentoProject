using System.Collections.Generic;

namespace StudentoMainProject.API.Models
{
    public class SubjectInstanceObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public UserObject Teacher { get; set; }
        public ICollection<UserObject> Students { get; set; }
    }
}
