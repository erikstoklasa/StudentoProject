using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;

namespace SchoolGradebook.Pages.Admin.Subjects.SubjectTypes
{
    public class EditModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public EditModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubjectType SubjectType { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectType = await _context.SubjectTypes.FirstOrDefaultAsync(m => m.Id == id);

            if (SubjectType == null)
            {
                return NotFound();
            }
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(SubjectType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectTypeExists(SubjectType.Id))
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

        private bool SubjectTypeExists(int id)
        {
            return _context.SubjectTypes.Any(e => e.Id == id);
        }
    }
}
