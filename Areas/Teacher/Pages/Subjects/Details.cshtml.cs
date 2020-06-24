using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using System.Security.Claims;
using SchoolGradebook.Services;

namespace SchoolGradebook.Areas.Teacher.Pages.Subjects
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
        public double[] StudentAverages { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Subject = await _analytics.GetSubjectAsync((int)id);

            if (Subject == null || Subject.Teacher.UserAuthId != UserId)
            {
                return NotFound();
                //Subject not found or access not permitted
            }

            Students = await _analytics.GetAllStudentsBySubjectIdAsync(Subject.Id);
            StudentAverages = new double[Students.Length];

            for (int i = 0; i < Students.Length; i++)
            {
                StudentAverages[i] = await _analytics.GetSubjectAverageForStudentByStudentIdAsync(Students[i].Id, (int)id);
            }

            return Page();
        }
    }
}
