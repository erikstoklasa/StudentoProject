using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace SchoolGradebook.Areas.Teacher.Pages.Grades
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;
        public List<SelectListItem> Students { get; private set; }
        private string UserId { get; set; }
        public string SubjectName { get; set; }

        [BindProperty(SupportsGet = true)]
        public int SubjectId { get; set; }

        public CreateModel(SchoolContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }

        public IActionResult OnGet()
        {
            Subject s = _context.Subjects
                .Find(SubjectId);
            SubjectName = s.Name;
            Enrollment[] enrollments = _context.Enrollments
                .Include(s => s.Student)
                .Where(s => s.SubjectId == SubjectId)
                .ToArray();
            Students = new List<SelectListItem> { };
            for (int i = 0; i < enrollments.Length; i++)
            {
                if (enrollments[i].StudentId == StudentId || StudentId == 0)
                {
                    Students.Add(
                    new SelectListItem(
                        enrollments[i].Student.getFullName(),
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
            Grade.Added = DateTime.UtcNow;
            Subject[] teachersSubjects = _context.Subjects
                .Where(s => s.Teacher.UserAuthId == UserId)
                .ToArray();
            if (teachersSubjects == null)//Teacher has no subjects assigned
            {
                return Page();
            }
            bool teacherCanAdd = false;
            foreach (Subject s in teachersSubjects)
            {
                if (s.Id == SubjectId)
                {
                    teacherCanAdd = true;
                }
            }
            if (!teacherCanAdd)
            {
                return Page();
            }
            if (Grade.Value == null)
            {
                return Page();
            }
            //In this state teacher is elegible for adding the grade to the specified subject
            Grade.SubjectId = SubjectId;
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Grades.Add(Grade);
            await _context.SaveChangesAsync();

            return LocalRedirect($"~/Teacher/Subjects/Details?id={ SubjectId }");
        }
    }
}
