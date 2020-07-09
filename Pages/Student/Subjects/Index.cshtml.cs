using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Student.Subjects
{
    public class IndexModel : PageModel
    {
        public string UserId { get; set; }
        private readonly Analytics _analytics;

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            _analytics = analytics;
        }

        public IList<Subject> Subjects { get;set; }

        public async Task OnGetAsync()
        {
            Subjects = await _analytics.GetAllSubjectsByStudentUserAuthAsync(UserId);
        }
    }
}
