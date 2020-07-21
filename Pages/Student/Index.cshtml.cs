using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public List<Grade> RecentGrades { get; set; }
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
        }
        public async Task OnGet()
        {
            int studentId = await studentService.GetStudentId(UserId);
            StudentFirstName = (await studentService.GetStudentAsync(studentId)).FirstName;
            RecentGrades = await gradeService.GetAllGradesByStudentAsync(studentId, 0, 5);
            Subjects = await subjectService.GetAllSubjectInstancesByStudentAsync(studentId);
            foreach (SubjectInstance si in Subjects)
            {
                //Update analytics method to use only studentId instead of UserID
                double sAvg = await _analytics.GetSubjectAverageForStudentAsync(UserId, si.Id);
                string output = sAvg.CompareTo(double.NaN) == 0 ? "Žádné známky" : sAvg.ToString("f2");
                SubjectAverages.Add(output);
            }
            SubjectsAndSubjectAverages = Subjects.Zip(SubjectAverages, (s, sa) => (s, sa));

            double currentAvg = await _analytics.GetTotalAverageAsync(UserId);
            GPA = currentAvg.CompareTo(double.NaN) == 0 ? "Žádné známky" : currentAvg.ToString("f2");

            double comparisonAvg = await _analytics.GetTotalAverageAsync(UserId, 365, 30);
            ViewData["ComparisonString"] = LanguageHelper.getAverageComparisonString(currentAvg, comparisonAvg);
        }
    }
}
