using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Areas.Admin.Pages.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private readonly Analytics _analytics;

        public CreateModel(SchoolGradebook.Data.SchoolContext context, Analytics analytics)
        {
            _context = context;
            StudentsSelectList = new List<SelectListItem> { };
            SubjectsSelectList = new List<SelectListItem> { };
            _analytics = analytics;
        }
        public List<SelectListItem> StudentsSelectList { get; private set; }
        public List<SelectListItem> SubjectsSelectList { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }
        public List<Models.Student> Students { get; set; }
        public List<Models.Subject> Subjects { get; set; }
        public List<Models.Subject> StudentSubjects { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Students = new List<Models.Student>{ await _analytics.GetStudentByIdAsync(StudentId) };
            Subjects = (await _analytics.GetAllSubjects()).ToList();
            StudentSubjects = (await _analytics.GetAllSubjectsByStudentIdAsync(StudentId)).ToList();

            StudentsSelectList.Add(new SelectListItem(Students[0].getFullName(), Students[0].Id.ToString()));

            foreach (Models.Subject sub in Subjects)
            {
                if (!sub.Enrollments.Where(s => s.StudentId == StudentId).Any())
                {
                    SubjectsSelectList.Add(new SelectListItem(sub.Name, sub.Id.ToString()));
                }
            }

            return Page();
        }

        [BindProperty]
        public Enrollment Enrollment { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Enrollments.Add(Enrollment);
            await _context.SaveChangesAsync();

            return LocalRedirect($"~/Admin/Students/Details?id={StudentId}");
        }
    }
}
