using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Teacher
{
    public class IndexModel : PageModel
    {
        private readonly Analytics _analytics;
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;

        public int StudentCount { get; set; }
        public int UniqueStudentCount { get; set; }
        public int SubjectCount { get; set; }
        private string UserId { get; set; }

        public IndexModel(Analytics analytics, IHttpContextAccessor httpContextAccessor, TeacherService teacherService, StudentService studentService)
        {
            _analytics = analytics;
            this.teacherService = teacherService;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
        public async Task OnGetAsync()
        {
            StudentCount = await _analytics.GetStudentsCountByTeacherUserAuthIdAsync(UserId);
            SubjectCount = await _analytics.GetSubjectsCountByTeacherIdAsync(UserId);

            int teacherId = await teacherService.GetTeacherId(UserId);
            UniqueStudentCount = (await studentService.GetAllStudentsByTeacherAsync(teacherId)).Count;
        }
    }
}
