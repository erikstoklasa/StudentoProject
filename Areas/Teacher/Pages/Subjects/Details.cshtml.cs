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

namespace SchoolGradebook.Areas.Teacher.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;
        private Analytics _analytics;

        public DetailsModel(SchoolGradebook.Data.SchoolContext context, IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _context = context;
            _analytics = analytics;
        }

        public string UserId { get; private set; }
        public Subject Subject { get; set; }
        public Models.Student[] Students { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject = await _context.Subjects
                .Include(s => s.Teacher)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (Subject == null)
            {
                return NotFound();
            }
            Students = await _analytics.getAllStudentsBySubjectIdAsync(Subject.Id);

            return Page();
        }
    }
}
