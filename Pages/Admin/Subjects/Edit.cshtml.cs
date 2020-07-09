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
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Subjects
{
    public class EditModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public List<SelectListItem> TeacherSelectList { get; set; }
        private readonly Analytics _analytics;
        public EditModel(SchoolGradebook.Data.SchoolContext context, Analytics analytics)
        {
            _context = context;
            _analytics = analytics;
            TeacherSelectList = new List<SelectListItem>();
        }

        [BindProperty]
        public Subject Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            Subject = await _context.Subjects
                .Include(s => s.Teacher).FirstOrDefaultAsync(m => m.Id == id);

            foreach (Models.Teacher t in await _analytics.GetAllTeachersAsync())
            {
                if(Subject.TeacherId == t.Id)
                {
                    TeacherSelectList.Add(new SelectListItem() { Text = t.getFullName(), Value = t.Id.ToString(), Selected = true });
                } else
                {
                    TeacherSelectList.Add(new SelectListItem() { Text = t.getFullName(), Value = t.Id.ToString() });
                }
                
            }

            if (Subject == null)
            {
                return NotFound();
            }

            ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "Id");
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

            _context.Attach(Subject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SubjectExists(Subject.Id))
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

        private bool SubjectExists(int id)
        {
            return _context.Subjects.Any(e => e.Id == id);
        }
    }
}
