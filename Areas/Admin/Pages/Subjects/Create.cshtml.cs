using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Data;
using SchoolGradebook.Models;

namespace SchoolGradebook.Areas.Admin.Pages.Subjects
{
    public class CreateModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public CreateModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "LastName");
            return Page();
        }

        [BindProperty]
        public Subject Subject { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Subjects.Add(Subject);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
