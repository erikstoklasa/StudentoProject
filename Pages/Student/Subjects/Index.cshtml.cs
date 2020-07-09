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
        private readonly Analytics analytics;

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics _analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            analytics = _analytics;
        }

        public IList<Subject> Subjects { get;set; }

        public async Task OnGetAsync()
        {
            Subjects = await analytics.GetAllSubjectsByStudentUserAuthAsync(UserId);
        }
    }
}
