using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Enrollments
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolContext _context;
        private readonly SubjectService subjectService;

        public DeleteModel(SchoolContext context, SubjectService subjectService)
        {
            _context = context;
            this.subjectService = subjectService;
        }

        [BindProperty]
        public Enrollment Enrollment { get; set; }
        public SubjectInstance Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Enrollment = await _context.Enrollments
                .Include(e => e.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            Subject = await subjectService.GetSubjectInstanceAsync(Enrollment.SubjectInstanceId);
            if (Enrollment == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Enrollment = await _context.Enrollments.FindAsync(id);

            if (Enrollment != null)
            {
                _context.Enrollments.Remove(Enrollment);
                await _context.SaveChangesAsync();
            }

            return LocalRedirect($"~/Admin/Students/Details?id={Enrollment.StudentId}");
        }
    }
}
