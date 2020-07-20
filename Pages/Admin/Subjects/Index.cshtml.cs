using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Subjects
{
    public class IndexModel : PageModel
    {
        public string UserId { get; set; }
        public List<SubjectInstance> SubjectInstances { get; set; }
        private readonly Analytics _analytics;
        private readonly SubjectService subjectService;

        public int[] StudentsCount { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics, SubjectService subjectService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            _analytics = analytics;
            this.subjectService = subjectService;
        }

        public async Task OnGetAsync()
        {
            SubjectInstances = await subjectService.GetAllSubjectInstancesAsync();
            StudentsCount = new int[SubjectInstances.Count];
            for (int i = 0; i < SubjectInstances.Count; i++)
            {
                StudentsCount[i] = await _analytics.GetStudentsCountInSubjectAsync(SubjectInstances[i].Id);
            }
        }
    }
}
