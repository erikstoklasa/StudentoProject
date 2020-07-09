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

namespace SchoolGradebook.Pages.Admin.Subjects
{
    public class CreateModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public List<SelectListItem> TeacherSelectList { get; set; }
        private readonly Analytics _analytics;

        public CreateModel(SchoolGradebook.Data.SchoolContext context, Analytics analytics)
        {
            _context = context;
            TeacherSelectList = new List<SelectListItem>();
            _analytics = analytics;
        }

        public async Task<IActionResult> OnGet()
        {
            foreach (Models.Teacher t in await _analytics.GetAllTeachersAsync())
            {
                TeacherSelectList.Add(new SelectListItem() { Text = t.getFullName(), Value = t.Id.ToString() });
            }
            //ViewData["TeacherId"] = new SelectList(_context.Teachers, "Id", "LastName");
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
