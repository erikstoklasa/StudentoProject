using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using StudentoMainProject.Models;
using StudentoMainProject.Services;

namespace SchoolGradebook.Pages.Teacher
{
    public class IndexModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;
        private readonly LogItemService logItemService;

        public int UniqueStudentCount { get; set; }
        public int SubjectCount { get; set; }
        private string UserId { get; set; }
        public List<int> StudentsCount { get; set; }
        public IEnumerable<(SubjectInstance subjectInstance, int studentCount)> SubjectsAndStudentCounts;
        public List<SubjectInstance> Subjects { get; set; }
        public IPAddress IPAddress { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor,
                          TeacherService teacherService,
                          StudentService studentService,
                          SubjectService subjectService,
                          LogItemService logItemService)
        {
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            this.logItemService = logItemService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            IPAddress = httpContextAccessor.HttpContext.Connection.RemoteIpAddress;
            StudentsCount = new List<int>();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            await logItemService.Log(
                new LogItem
                {
                    EventType = "TeacherIndex",
                    Timestamp = new DateTime(),
                    UserAuthId = UserId,
                    UserId = teacherId,
                    UserRole = "teacher",
                    IPAddress = IPAddress.ToString()
                });

            Subjects = await subjectService.GetAllSubjectInstancesByTeacherAsync(teacherId);

            foreach (SubjectInstance si in Subjects)
            {
                StudentsCount.Add(await studentService.GetStudentCountBySubjectAsync(si.Id));
            }
            SubjectsAndStudentCounts = Subjects.Zip(StudentsCount, (si, sc) => (si, sc));
            if (teacherId == -1)
            {
                return LocalRedirect("/ActivateAccount");
            }
            return Page();
        }
    }
}
