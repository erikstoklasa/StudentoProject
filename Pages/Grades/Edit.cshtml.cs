using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;

namespace SchoolGradebook.Pages.Grades
{
    public class EditModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        public List<SelectListItem> Subjects { get; private set; }
        public List<SelectListItem> Students { get; private set; }

        public EditModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
            List<Student> tempStud = _context.Students.ToList();
            List<Subject> tempSubj = _context.Subjects.ToList();
            Subjects = new List<SelectListItem> {};
            Students = new List<SelectListItem> {};
            foreach (Student s in tempStud)
            {
                Students.Add(new SelectListItem(s.FirstName + " " + s.LastName, s.Id.ToString()));
            }
            foreach (Subject s in tempSubj)
            {
                Subjects.Add(new SelectListItem(s.Name, s.Id.ToString()));
            }

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
