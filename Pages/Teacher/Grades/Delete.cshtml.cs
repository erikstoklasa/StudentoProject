using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Grades
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public DeleteModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Grade Grade { get; set; }

        [BindProperty(SupportsGet = true)]
        public int GradeId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject.Teacher)
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
