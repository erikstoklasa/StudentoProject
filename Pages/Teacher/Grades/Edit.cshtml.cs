using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Grades
{
    public class EditModel : PageModel
    {
        private readonly SchoolContext _context;
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;

        public List<SelectListItem> Subjects { get; private set; }
        public List<SelectListItem> Students { get; private set; }
        public string UserId { get; set; }

        public EditModel(SchoolContext context, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            List<Models.Student> tempStud = _context.Students.ToList();
            List<SubjectInstance> tempSubj = _context.Subjects.ToList();
            Subjects = new List<SelectListItem> {};
            Students = new List<SelectListItem> {};

            foreach (Models.Student s in tempStud)
            {
                Students.Add(new SelectListItem(s.getFullName(), s.Id.ToString()));
            }
            foreach (SubjectInstance s in tempSubj)
            {
                Subjects.Add(new SelectListItem(s.Name, s.Id.ToString()));
            }
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

        }

        [BindProperty]
        public Grade Grade { get; set; }
        [BindProperty]
        public int gradeId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if(gradeId == null)
            {
                return NotFound();
            }
            int teahcerId = await teacherService.GetTeacherId(UserId);
            bool hasAccessToGrade = await teacherAccessValidation.HasAccessToGrade(teahcerId, gradeId);
            if (!hasAccessToGrade)
            {
                return NotFound();
            }
            Grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.SubjectInstance).FirstOrDefaultAsync(m => m.Id == gradeId);

            if (Grade == null)
            {
                return NotFound();
            }

            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Grade).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GradeExists(Grade.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool GradeExists(int id)
        {
            return _context.Grades.Any(e => e.Id == id);
        }
    }
}
