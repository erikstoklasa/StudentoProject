using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class DetailsModel : PageModel
    {
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;

        public DetailsModel(StudentService studentService, SubjectService subjectService)
        {
            this.studentService = studentService;
            this.subjectService = subjectService;
        }

        public Models.Student Student { get; set; }
        public List<Models.SubjectInstance> SubjectInstances { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await studentService.GetStudentFullProfileAsync((int)id);

            if (Student == null)
            {
                return NotFound();
            }

            SubjectInstances = await subjectService.GetAllSubjectInstancesByStudentAsync((int)id);

            return Page();
        }
    }
}
