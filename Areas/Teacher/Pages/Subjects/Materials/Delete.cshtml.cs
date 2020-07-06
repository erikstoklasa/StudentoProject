using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;

namespace SchoolGradebook.Areas.Teacher.Pages.Subjects.Materials
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public DeleteModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SubjectMaterial SubjectMaterial { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectMaterial = await _context.SubjectMaterials
                .Include(s => s.Subject).FirstOrDefaultAsync(m => m.Id == id);

            if (SubjectMaterial == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectMaterial = await _context.SubjectMaterials.FindAsync(id);
            int subjectId = SubjectMaterial.SubjectId;

            if (SubjectMaterial != null)
            {
                _context.SubjectMaterials.Remove(SubjectMaterial);
                await _context.SaveChangesAsync();
            }

            return LocalRedirect($"~/Teacher/Subjects/Details?id={ subjectId }");
        }
    }
}
