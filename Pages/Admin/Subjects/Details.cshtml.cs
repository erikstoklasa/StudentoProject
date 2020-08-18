using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using System.Security.Claims;
using SchoolGradebook.Services;
using System.Collections.Generic;

namespace SchoolGradebook.Pages.Admin.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;

        public DetailsModel(IHttpContextAccessor httpContextAccessor, StudentService studentService, SubjectService subjectService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.studentService = studentService;
            this.subjectService = subjectService;
        }

        public string UserId { get; private set; }
        public SubjectInstance Subject { get; set; }
        public Models.Student[] Students { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject = await subjectService.GetSubjectInstanceFullAsync((int)id);

            if (Subject == null)
            {
                return NotFound();
            }

            Students = await studentService.GetAllStudentsBySubjectInstanceAsync(Subject.Id);

            return Page();
        }
    }
}
