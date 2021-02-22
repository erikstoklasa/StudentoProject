using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Student.Grades
{
    public class CreateModel : PageModel
    {
        private readonly SubjectService subjectService;
        private readonly StudentAccessValidation studentAccessValidation;
        private readonly StudentService studentService;
        private readonly GradeService gradeService;

        public string ErrorMessage { get; set; }
        public string SuccessMessage { get; set; }
        public SubjectInstance SubjectInstance { get; set; }
        public string UserId { get; set; }
        [BindProperty]
        public Grade Grade { get; set; }
        public CreateModel(SubjectService subjectService, StudentAccessValidation studentAccessValidation, IHttpContextAccessor httpContextAccessor, StudentService studentService, GradeService gradeService)
        {
            this.subjectService = subjectService;
            this.studentAccessValidation = studentAccessValidation;
            this.studentService = studentService;
            this.gradeService = gradeService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task<IActionResult> OnGetAsync(int subjectInstanceId)
        {
            int studentId = await studentService.GetStudentId(UserId);
            bool hasAccess = await studentAccessValidation.HasAccessToSubject(studentId, subjectInstanceId);
            if (!hasAccess)
            {
                return Forbid();
            }
            SubjectInstance = await subjectService.GetSubjectInstanceAsync(subjectInstanceId);
            Grade = new Grade();
            Grade.SubjectInstanceId = SubjectInstance.Id;
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            int studentId = await studentService.GetStudentId(UserId);
            Grade.StudentId = studentId;

            bool hasAccess = await studentAccessValidation.HasAccessToSubject(studentId, Grade.SubjectInstanceId);
            if (!hasAccess)
            {
                return Forbid();
            }

            Grade.Added = DateTime.UtcNow;

            try
            {
                await gradeService.AddGradeAsync(Grade, Grade.USERTYPE.Student);
            }
            catch (ArgumentNullException e)
            {
                ErrorMessage = e.Message;
                return await OnGetAsync(Grade.SubjectInstanceId);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorMessage = e.Message;
                return await OnGetAsync(Grade.SubjectInstanceId);
            }
            SuccessMessage = $"Známka {Grade.Value} - {Grade.Name} byla přidána";
            return await OnGetAsync(Grade.SubjectInstanceId);
        }
    }
}
