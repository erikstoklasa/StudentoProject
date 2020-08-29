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

namespace SchoolGradebook.Pages.Teacher
{
    public class IndexModel : PageModel
    {
        private readonly Analytics _analytics;
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;
        private readonly SubjectService subjectService;

        public int StudentCount { get; set; }
        public int UniqueStudentCount { get; set; }
        public int SubjectCount { get; set; }
        private string UserId { get; set; }
        public List<int> StudentsCount { get; set; }
        public IEnumerable<(SubjectInstance subjectInstance, int studentCount)> SubjectsAndStudentCounts;
        public List<SubjectInstance> Subjects { get; set; }

        public IndexModel(Analytics analytics, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, StudentService studentService, SubjectService subjectService)
        {
            _analytics = analytics;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.subjectService = subjectService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            StudentsCount = new List<int>();
        }
        public async Task<IActionResult> OnGetAsync()
        {
            StudentCount = await _analytics.GetStudentsCountByTeacherUserAuthIdAsync(UserId);
            SubjectCount = await _analytics.GetSubjectsCountByTeacherIdAsync(UserId);

            int teacherId = await teacherService.GetTeacherId(UserId);

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
            UniqueStudentCount = (await studentService.GetAllStudentsByTeacherAsync(teacherId)).Count;
            return Page();
        }
    }
}
