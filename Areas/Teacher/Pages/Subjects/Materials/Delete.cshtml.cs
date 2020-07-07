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

namespace SchoolGradebook.Areas.Teacher.Pages.Subjects.Materials
{
    public class DeleteModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private readonly Analytics _analytics;

        public DeleteModel(SchoolGradebook.Data.SchoolContext context, Analytics analytics)
        {
            _context = context;
            _analytics = analytics;
        }

        [BindProperty]
        public SubjectMaterial SubjectMaterial { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            SubjectMaterial = await _analytics.GetSubjectMaterialAsync((Guid)id);

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
