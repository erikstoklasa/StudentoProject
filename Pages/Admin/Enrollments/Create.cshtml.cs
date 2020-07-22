using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Enrollments
{
    public class CreateModel : PageModel
    {
        private readonly SchoolContext _context;
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;

        public CreateModel(SchoolContext context, SubjectService subjectService, StudentService studentService)
        {
            _context = context;
            StudentsSelectList = new List<SelectListItem> { };
            SubjectsSelectList = new List<SelectListItem> { };
            this.subjectService = subjectService;
            this.studentService = studentService;
        }
        public List<SelectListItem> StudentsSelectList { get; private set; }
        public List<SelectListItem> SubjectsSelectList { get; private set; }
        [BindProperty(SupportsGet = true)]
        public int StudentId { get; set; }
        public List<Models.Student> Students { get; set; }
        public List<Models.SubjectInstance> Subjects { get; set; }
        public List<Models.SubjectInstance> StudentSubjects { get; set; }
        public async Task<IActionResult> OnGet()
        {
            Students = new List<Models.Student> { await studentService.GetStudentAsync(StudentId) };
            Subjects = await subjectService.GetAllSubjectInstancesFullAsync();
            StudentSubjects = await subjectService.GetAllSubjectInstancesByStudentAsync(StudentId);

            StudentsSelectList.Add(new SelectListItem(Students[0].GetFullName(), Students[0].Id.ToString()));

            foreach (Models.SubjectInstance sub in Subjects)
            {
                if (!sub.Enrollments.Where(s => s.StudentId == StudentId).Any())
                {
                    SubjectsSelectList.Add(new SelectListItem(sub.GetFullName(), sub.Id.ToString()));
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
