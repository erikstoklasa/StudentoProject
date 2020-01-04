using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;

namespace SchoolGradebook.Pages.Grades
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
        [BindProperty]
        public int gradeId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject).FirstOrDefaultAsync(m => m.Id == gradeId);

            if (Grade == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int gradeId)
        {

            Grade = await _context.Grades.FindAsync(gradeId);

            if (Grade != null)
            {
                _context.Grades.Remove(Grade);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
