using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Teacher.Subjects
{
    public class IndexModel : PageModel
    {
        public string UserId { get; set; }
        public SubjectInstance[] Subjects { get; set; }
        private readonly Analytics _analytics;
        private readonly StudentService studentService;

        public int[] StudentsCount { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, Analytics analytics, StudentService studentService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            _analytics = analytics;
            this.studentService = studentService;
        }

        public async Task OnGetAsync()
        {
            Subjects = await _analytics.GetAllSubjectsByTeacherUserAuthAsync(UserId);
            StudentsCount = new int[Subjects.Length];
            for (int i = 0; i < Subjects.Length; i++)
            {
                StudentsCount[i] = await studentService.GetStudentCountBySubjectAsync(Subjects[i].Id);
            }
        }
    }
}
