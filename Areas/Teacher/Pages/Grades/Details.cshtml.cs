using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;

namespace SchoolGradebook.Areas.Teacher.Pages.Grades
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public DetailsModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        public Grade Grade { get; set; }

        [BindProperty(SupportsGet = true)]
        public int gradeId { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            Grade = await _context.Grades
                .Include(g => g.Student)
                .Include(g => g.Subject.Teacher)
                .Include(g => g.Subject).FirstOrDefaultAsync(m => m.Id == gradeId);

            if (Grade == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
