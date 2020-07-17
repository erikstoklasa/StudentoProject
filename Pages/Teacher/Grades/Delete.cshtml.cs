using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Grades
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        public string UserId { get; set; }
        public int TeacherId { get; set; }

        public DeleteModel(Data.SchoolContext context, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation)
        {
            _context = context;
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [BindProperty]
        public Grade Grade { get; set; }

        [BindProperty(SupportsGet = true)]
        public int GradeId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            TeacherId = await teacherService.GetTeacherId(UserId);
            bool hasAccessToGrade = await teacherAccessValidation.HasAccessToGrade(TeacherId, GradeId);
            if (!hasAccessToGrade)
            {
                return NotFound();
            }
            Grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.SubjectInstance.Teacher)
                .Where(g => g.Id == GradeId)
                .FirstOrDefaultAsync();

            if (Grade == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            TeacherId = await teacherService.GetTeacherId(UserId);
            bool hasAccessToGrade = await teacherAccessValidation.HasAccessToGrade(TeacherId, Grade.Id);
            if (!hasAccessToGrade)
            {
                return NotFound();
            }
            Grade = await _context.Grades.FindAsync(Grade.Id);

            if (Grade != null)
            {
                _context.Grades.Remove(Grade);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
