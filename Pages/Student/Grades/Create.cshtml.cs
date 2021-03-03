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
    public class Grade
    {
        public int SubjectInstanceId { get; set; }
        public string Value { get; set; }
        public string Name { get; set; }
    }
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
            Models.Grade grade = new Models.Grade
            {
                SubjectInstanceId = Grade.SubjectInstanceId,
                StudentId = studentId,
                Name = Grade.Name
            };

            bool hasAccess = await studentAccessValidation.HasAccessToSubject(studentId, Grade.SubjectInstanceId);
            if (!hasAccess)
            {
                return Forbid();
            }

            grade.Added = DateTime.UtcNow;
            try
            {
                grade.SetGradeValue(Grade.Value);
            }
            catch (ArgumentException e)
            {
                ErrorMessage = "Známka je ve špatném formátu. Povolené kombinace jsou: 1*;1+;1;1-;2+;2;2-;3+;3;3-;4+;4;4-;5+;5;5- " + e.Message;
                return await OnGetAsync(grade.SubjectInstanceId);
            }

            try
            {
                await gradeService.AddGradeAsync(grade, Models.Grade.USERTYPE.Student);
            }
            catch (ArgumentNullException e)
            {
                ErrorMessage = e.Message;
                return await OnGetAsync(grade.SubjectInstanceId);
            }
            catch (ArgumentOutOfRangeException e)
            {
                ErrorMessage = e.Message;
                return await OnGetAsync(grade.SubjectInstanceId);
            }
            SuccessMessage = $"Známka {grade.Name} - {grade.GetGradeValue()} byla přidána";
            return await OnGetAsync(grade.SubjectInstanceId);
        }
    }
}
