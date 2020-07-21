using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SchoolGradebook.Pages.Student.Grades
{
    public class DetailsModel : PageModel
    {
        private readonly GradeService gradeService;
        private readonly StudentAccessValidation studentAccessValidation;
        private readonly StudentService studentService;

        public string UserId { get; set; }
        public Grade Grade { get; set; }

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
    }
}
