using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class SubjectType
    {
        public int Id { set; get; }
        public String Name { set; get; }
        public String SpecializationName { set; get; }
        public int SchoolId { get; set; }

        public ICollection<SubjectMaterial> SubjectMaterials { set; get; }
        public ICollection<SubjectInstance> SubjectInstances { set; get; }
        public School School { get; set; }
    }
}
