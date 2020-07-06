using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Models
{
    public class SubjectMaterial
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required(ErrorMessage = "Zadejte název materiálu")]
        public string Name { get; set; }
        public string FileExt { get; set; }
        public DateTime Added { get; set; }

        public Subject Subject { get; set; }
    }
}
