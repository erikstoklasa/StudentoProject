using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Student.Subjects
{
    public class IndexModel : PageModel
    {
        public string UserId { get; set; }
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;
        private readonly Analytics analytics;

        public IndexModel(IHttpContextAccessor httpContextAccessor, SubjectService subjectService, StudentService studentService, Analytics analytics)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            this.subjectService = subjectService;
            this.studentService = studentService;
            this.analytics = analytics;
            SubjectAverages = new List<string>();
        }

        public List<SubjectInstance> Subjects { get;set; }
        public List<string> SubjectAverages { get; set; }
        public IEnumerable<(SubjectInstance subjectInstance, string subjectAverage)> SubjectsAndSubjectAverages { get; set; }

        public async Task OnGetAsync()
        {
            int studentId = await studentService.GetStudentId(UserId);
            Subjects = await subjectService.GetAllSubjectInstancesByStudentAsync(studentId);
            foreach(SubjectInstance si in Subjects)
            {
                //Update analytics method to use only studentId instead of UserID
                double sAvg = await analytics.GetSubjectAverageForStudentAsync(UserId, si.Id);
                string output = sAvg.CompareTo(double.NaN) == 0 ? "Žádné známky" : sAvg.ToString("f2");
                SubjectAverages.Add(output);
            }
            SubjectsAndSubjectAverages = Subjects.Zip(SubjectAverages, (s, sa) => (s, sa));
        }
    }
}
