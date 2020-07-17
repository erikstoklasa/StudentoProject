using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Students
{
    public class IndexModel : PageModel
    {
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;

        public string UserId { get; set; }

        public IndexModel(StudentService studentService, TeacherService teacherService, IHttpContextAccessor httpContextAccessor)
        {
            this.studentService = studentService;
            this.teacherService = teacherService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        public List<Models.Student> Students { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            int teacherId = await teacherService.GetTeacherId(UserId);
            Students = await studentService.GetAllStudentsByTeacherAsync(teacherId);
            return Page();
        }
    }
}
