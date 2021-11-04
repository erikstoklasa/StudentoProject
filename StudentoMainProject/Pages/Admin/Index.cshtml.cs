using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin
{
    public class IndexModel : PageModel
    {
        public double StudentToTeacherRatio { get; set; }
        public string UserId { get; set; }
        public int StudentCount { get; set; }
        public int TeacherCount { get; set; }
        private readonly AdminService adminService;
        private readonly StudentService studentService;
        private readonly TeacherService teacherService;

        public IndexModel(IHttpContextAccessor httpContextAccessor, AdminService adminService, StudentService studentService, TeacherService teacherService)
        {
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
            this.adminService = adminService;
            this.studentService = studentService;
            this.teacherService = teacherService;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            bool adminIsSufficientLevel = false;
            try
            {
                int adminId = await adminService.GetAdminId(UserId);
                adminIsSufficientLevel = await adminService.IsAdminSufficientLevel(adminId, 1);
            }
            catch
            {
                return Forbid();
            }

            if (!adminIsSufficientLevel)
            {
                return Forbid();
            }
            TeacherCount = await teacherService.GetTeacherCountAsync();
            StudentCount = await studentService.GetStudentCountAsync();            
            StudentToTeacherRatio = (double)StudentCount / TeacherCount;
            return Page();
        }
    }
}
