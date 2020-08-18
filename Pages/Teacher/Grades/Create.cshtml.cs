using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Grades
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;
        public List<SelectListItem> Students { get; private set; }
        private string UserId { get; set; }
        public string SubjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SubjectInstanceId { get; set; }
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly GradeService gradeService;
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;

        public int TeacherId { get; set; }

        public CreateModel(SchoolContext context, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation, GradeService gradeService, SubjectService subjectService, StudentService studentService)
        {
            _context = context;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.gradeService = gradeService;
            this.subjectService = subjectService;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public async Task<IActionResult> OnGet()
        {
            TeacherId = await teacherService.GetTeacherId(UserId);
            bool hasAccessToSubject = await teacherAccessValidation.HasAccessToSubject(TeacherId, SubjectInstanceId);
            if (!hasAccessToSubject)
            {
                return Forbid();
            }

            SubjectInstance s = await subjectService.GetSubjectInstanceAsync(SubjectInstanceId);
            SubjectName = s.SubjectType.Name;

            foreach(Models.Student student in await studentService.GetAllStudentsBySubjectInstanceAsync(SubjectInstanceId))
            {
                if(student.Id == StudentId || StudentId == 0)
                {
                    Students.Add(
                    new SelectListItem(
                        s.GetFullName(),
                        s.Id.ToString())
                    );
                }
            }

            return Page();
        }

        [BindProperty]
        public Grade Grade { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            TeacherId = await teacherService.GetTeacherId(UserId);
            bool hasAccessToSubject = await teacherAccessValidation.HasAccessToSubject(TeacherId, SubjectInstanceId);
            if (!hasAccessToSubject)
            {
                return Forbid();
            }

            if (Grade.Value == null)
            {
                return Page();
            }

            //Validation complete
            Grade.Added = DateTime.UtcNow;
            Grade.SubjectInstanceId = SubjectInstanceId;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await gradeService.AddGradeAsync(Grade);

            return LocalRedirect($"~/Teacher/Subjects/Details?id={ SubjectInstanceId }");
        }
    }
}
