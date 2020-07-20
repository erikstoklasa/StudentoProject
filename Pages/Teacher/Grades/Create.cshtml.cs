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
        public int TeacherId { get; set; }

        public CreateModel(SchoolContext context, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation)
        {
            _context = context;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
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
            SubjectInstance s = _context.SubjectInstances
                .Find(SubjectInstanceId);
            SubjectName = s.SubjectTypeId.ToString();
            Enrollment[] enrollments = _context.Enrollments
                .Include(s => s.Student)
                .Where(s => s.SubjectInstanceId == SubjectInstanceId)
                .ToArray();
            Students = new List<SelectListItem> { };
            for (int i = 0; i < enrollments.Length; i++)
            {
                if (enrollments[i].StudentId == StudentId || StudentId == 0)
                {
                    Students.Add(
                    new SelectListItem(
                        enrollments[i].Student.GetFullName(),
                        enrollments[i].StudentId.ToString())
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

            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();

            return LocalRedirect($"~/Teacher/Subjects/Details?id={ SubjectInstanceId }");
        }
    }
}
