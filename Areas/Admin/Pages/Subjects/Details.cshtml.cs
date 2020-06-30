using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using System.Security.Claims;
using SchoolGradebook.Services;
using System.Collections.Generic;

namespace SchoolGradebook.Areas.Admin.Pages.Subjects
{
    public class DetailsModel : PageModel
    {
        private readonly Analytics _analytics;

        public DetailsModel(IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
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

            Subject = await _analytics.GetSubjectAsync((int)id);

            if (Subject == null)
            {
                return NotFound();
            }

            Students = await _analytics.GetAllStudentsBySubjectIdAsync(Subject.Id);

            return Page();
        }
    }
}
