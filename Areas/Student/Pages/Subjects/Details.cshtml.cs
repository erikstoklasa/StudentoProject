using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using System.Security.Claims;
using SchoolGradebook.Services;

namespace SchoolGradebook.Areas.Student.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private readonly Analytics _analytics;
        public SubjectMaterial[] SubjectMaterials { get; set; }

        public DetailsModel(SchoolGradebook.Data.SchoolContext context, IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context = context;
            _analytics = analytics;
        }

        public string UserId { get; private set; }
        public Subject Subject { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject = await _context.Subjects
                .Include(s => s.Teacher).FirstOrDefaultAsync(m => m.Id == id);
            SubjectMaterials = await _analytics.GetAllSubjectMaterialsAsync((int)id);

            double currentAvg = await _analytics.GetSubjectAverageForStudentAsync(UserId, Subject.Id);
            double comparisonAvg = await _analytics.GetSubjectAverageForStudentAsync(UserId, Subject.Id, 365, 30);
            ViewData["ComparisonString"] = LanguageHelper.getAverageComparisonString(currentAvg, comparisonAvg);

            if (Subject == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
