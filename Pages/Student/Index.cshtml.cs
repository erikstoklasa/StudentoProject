using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Student
{
    public class IndexModel : PageModel
    {
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;
        private readonly Analytics _analytics;
        private readonly GradeService gradeService;

        public string StudentFirstName { get; set; }
        public string UserId { get; private set; }
        public List<SubjectInstance> Subjects { get; set; }
        public List<string> SubjectAverages { get; set; }
        public Grade[] RecentGrades { get; set; }
        public string GPA { get; set; }
        public IEnumerable<(SubjectInstance subjectInstance, string subjectAverage)> SubjectsAndSubjectAverages { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, SubjectService subjectService, StudentService studentService, Analytics analytics, GradeService gradeService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            this.subjectService = subjectService;
            this.studentService = studentService;
            _analytics = analytics;
            this.gradeService = gradeService;
            SubjectAverages = new List<string>();
            System.Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");
        }
        public async Task<IActionResult> OnGetAsync()
        {
            int studentId = await studentService.GetStudentId(UserId);
            if (studentId == -1)
            {
                return LocalRedirect("/ActivateAccount");
            }
            StudentFirstName = (await studentService.GetStudentBasicInfoAsync(studentId)).FirstName;
            RecentGrades = await gradeService.GetRecentGradesByStudentAsync(studentId, 0, 3);
            Subjects = await subjectService.GetAllSubjectInstancesByStudentAsync(studentId);
            foreach (SubjectInstance si in Subjects)
            {
                double sAvg = await _analytics.GetSubjectAverageForStudentAsync(studentId, si.Id);
                string output = sAvg.CompareTo(double.NaN) == 0 ? "" : sAvg.ToString("f2");
                SubjectAverages.Add(output);
            }
            SubjectsAndSubjectAverages = Subjects.Zip(SubjectAverages, (s, sa) => (s, sa));
            double currentAvg = await _analytics.GetTotalAverageForStudentAsync(studentId);
            GPA = currentAvg.CompareTo(double.NaN) == 0 ? "Žádné známky" : currentAvg.ToString("f2");

            double comparisonAvg = await _analytics.GetTotalAverageForStudentAsync(studentId, 365, 30);
            ViewData["ComparisonString"] = LanguageHelper.getAverageComparisonString(currentAvg, comparisonAvg);
            return Page();
        }
    }
}
