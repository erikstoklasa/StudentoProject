using Google.Apis.Classroom.v1.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using StudentoMainProject.Models;
using StudentoMainProject.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Student
{
    public class IndexModel : PageModel
    {
        private readonly SubjectService subjectService;
        private readonly StudentService studentService;
        private readonly AnalyticsService _analytics;
        private readonly GradeService gradeService;
        private readonly LogItemService logItemService;

        public string StudentFirstName { get; set; }
        public string UserId { get; private set; }
        public List<SubjectInstance> Subjects { get; set; }
        public List<string> SubjectAverages { get; set; }
        public Grade[] RecentGrades { get; set; }
        public string GPAToDisplay { get; set; }
        public double GPA { get; set; }
        public IEnumerable<(SubjectInstance subjectInstance, string subjectAverage)> SubjectsAndSubjectAverages { get; set; }
        public ListCoursesResponse ListCoursesResponse { get; set; }
        public string GPAComparisonHTML { get; set; }
        public IList<Course> Courses { get; set; }
        public string ClassroomStatus { get; set; }
        public IPAddress IPAddress { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor,
                          SubjectService subjectService,
                          StudentService studentService,
                          AnalyticsService analytics,
                          GradeService gradeService,
                          LogItemService logItemService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            this.subjectService = subjectService;
            this.studentService = studentService;
            _analytics = analytics;
            this.gradeService = gradeService;
            this.logItemService = logItemService;
            SubjectAverages = new List<string>();
            Thread.CurrentThread.CurrentCulture = CultureInfo.GetCultureInfo("cs-CZ");
        }
        public async Task<IActionResult> OnGetAsync()
        {
            //string[] Scopes = { ClassroomService.Scope.ClassroomCoursesReadonly };
            //string ApplicationName = "Classroom API .NET Quickstart";
            int studentId = await studentService.GetStudentId(UserId);
            if (studentId == -1)
            {
                return LocalRedirect("/ActivateAccount");
            }
            await logItemService.Log(
                new LogItem
                {
                    EventType = "StudentIndex",
                    Timestamp = DateTime.UtcNow,
                    UserAuthId = UserId,
                    UserId = studentId,
                    UserRole = "student",
                    IPAddress = IPAddress.ToString()
                });
            StudentFirstName = (await studentService.GetStudentBasicInfoAsync(studentId)).FirstName;
            RecentGrades = await gradeService.GetRecentGradesAsync(studentId, 0, 3);
            Subjects = await subjectService.GetAllSubjectInstancesByStudentAsync(studentId);
            foreach (SubjectInstance si in Subjects)
            {
                double sAvg = AnalyticsService.GetSubjectAverageForStudentAsync(
                        await gradeService.GetAllGradesByStudentSubjectInstance(studentId, si.Id)
                    );
                string output = sAvg.CompareTo(double.NaN) == 0 ? "" : sAvg.ToString("f2");
                SubjectAverages.Add(output);
            }
            SubjectsAndSubjectAverages = Subjects.Zip(SubjectAverages, (s, sa) => (s, sa));
            double currentAvg = await _analytics.GetTotalAverageForStudentAsync(studentId);
            GPAToDisplay = currentAvg.CompareTo(double.NaN) == 0 ? "��dn� zn�mky" : currentAvg.ToString("f2");
            GPA = currentAvg;
            double comparisonAvg = await _analytics.GetTotalAverageForStudentAsync(studentId, 365, 30);
            GPAComparisonHTML = LanguageHelper.GetAverageComparisonString(currentAvg, comparisonAvg);


            //UserCredential credential;

            //using (var stream =
            //    new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            //{
            //    // The file token.json stores the user's access and refresh tokens, and is created
            //    // automatically when the authorization flow completes for the first time.
            //    string credPath = "token.json";
            //    credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
            //        GoogleClientSecrets.Load(stream).Secrets,
            //        Scopes,
            //        "user",
            //        CancellationToken.None,
            //        new FileDataStore(credPath, true)).Result;
            //}

            //// Create Classroom API service.
            //var service = new ClassroomService(new BaseClientService.Initializer()
            //{
            //    HttpClientInitializer = credential,
            //    ApplicationName = ApplicationName,
            //});

            //// Define request parameters.
            //CoursesResource.ListRequest request = service.Courses.List();
            //request.PageSize = 10;

            //// List courses.
            //ListCoursesResponse response = request.Execute();
            //if (response.Courses != null && response.Courses.Count > 0)
            //{
            //    Courses = response.Courses;
            //}
            //else
            //{
            //    ClassroomStatus = "No courses found";
            //}

            return Page();
        }
    }
}
