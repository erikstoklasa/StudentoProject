using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Areas.Teacher.Pages.Subjects
{
    public class IndexModel : PageModel
    {
        public string UserId { get; set; }
        public Subject[] Subjects { get; set; }
        private readonly Analytics _analytics;
        public int[] StudentsCount { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            _analytics = analytics;
        }

        public async Task OnGetAsync()
        {
            Subjects = await _analytics.GetAllSubjectsByTeacherUserAuthAsync(UserId);
            StudentsCount = new int[Subjects.Length];
            for (int i = 0; i < Subjects.Length; i++)
            {
                StudentsCount[i] = await _analytics.GetStudentsCountInSubjectAsync(Subjects[i].Id);
            }
        }
    }
}
