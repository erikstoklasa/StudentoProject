using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace SchoolGradebook.Pages.Student.Grades
{
    public class DetailsModel : PageModel
    {
        private readonly Analytics _analytics;
        public string UserId { get; set; }
        public Grade Grade { get; set; }

        [BindProperty(SupportsGet = true)]
        public int gradeId { get; set; }

        public DetailsModel(Analytics analytics, IHttpContextAccessor httpContextAccessor)
        {
            _analytics = analytics;

            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Grade = await _analytics.GetGradeAsync(UserId, gradeId);
            if (Grade == null)
            {
                return NotFound();
                //Not found or access not premitted
            }
            return Page();
        }
    }
}
