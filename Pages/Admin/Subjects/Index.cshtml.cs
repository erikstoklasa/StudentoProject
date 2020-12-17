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
        private readonly StudentService studentService;

        public List<int> StudentsCount { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics, SubjectService subjectService, StudentService studentService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            _analytics = analytics;
            this.subjectService = subjectService;
            this.studentService = studentService;
            StudentsCount = new List<int>();
        }

        public async Task OnGetAsync()
        {
            SubjectInstances = await subjectService.GetAllSubjectInstancesAsync();

            foreach (var si in SubjectInstances)
            {
                StudentsCount.Add(await studentService.GetStudentCountBySubjectAsync(si.Id));
            }
        }
    }
}
