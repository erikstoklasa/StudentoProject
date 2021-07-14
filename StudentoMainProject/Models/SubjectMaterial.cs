using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class SubjectMaterial
    {
        public enum USERTYPE { Teacher, Student }

        public Guid Id { get; set; }
        public int? SubjectTypeId { get; set; } //If set, then you will be able to see the material in a subject type overview
        public int SubjectInstanceId { set; get; }
        public int? SubjectMaterialGroupId { set; get; }

        [Required(ErrorMessage = "Zadejte název materiálu")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string FileExt { get; set; }
        public string FileType { get; set; } //MIME Media type https://developer.mozilla.org/en-US/docs/Web/HTTP/Basics_of_HTTP/MIME_types/Common_types
        public DateTime Added { get; set; }
        public USERTYPE AddedBy { get; set; }
        public int AddedById { get; set; }
        public bool ToDelete { get; set; }

        public SubjectType SubjectType { get; set; }
        public SubjectMaterialGroup SubjectMaterialGroup { get; set; }
        public Teacher Teacher { set; get; }
    }
}
