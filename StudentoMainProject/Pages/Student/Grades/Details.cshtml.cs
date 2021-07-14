using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Collections.Generic;

namespace SchoolGradebook.Pages.Student.Grades
{
    public class DetailsModel : PageModel
    {
        private readonly GradeService gradeService;
        private readonly StudentAccessValidation studentAccessValidation;
        private readonly StudentService studentService;

        public string UserId { get; set; }
        public Models.Grade Grade { get; set; }

        [BindProperty(SupportsGet = true)]
        public int gradeId { get; set; }

        public DetailsModel(IHttpContextAccessor httpContextAccessor, GradeService gradeService, StudentAccessValidation studentAccessValidation, StudentService studentService)
        {
            this.gradeService = gradeService;
            this.studentAccessValidation = studentAccessValidation;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            int studentId = await studentService.GetStudentId(UserId);
            bool hasAccessToGrade = await studentAccessValidation.HasAccessToGrade(studentId, gradeId);
            if (!hasAccessToGrade)
            {
                return BadRequest();
            }
            Grade = await gradeService.GetGradeAsync(gradeId);
            if (Grade == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            int studentId = await studentService.GetStudentId(UserId);
            bool hasAccessToGrade = await studentAccessValidation.HasAccessToGrade(studentId, (int)id);

            if (!hasAccessToGrade)
            {
                return BadRequest();
            }

            Grade = await gradeService.GetGradeAsync((int)id);

            if (Grade == null)
            {
                return NotFound();
            }

            await gradeService.DeleteGradesAsync(new List<int>()
            {
                (int)id
            });
            return RedirectToPage("/Student/Subjects/Details", new { id = Grade.SubjectInstanceId });
        }
    }
}
