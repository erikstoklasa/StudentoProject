using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Student.Grades
{
    public class IndexModel : PageModel
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly GradeService gradeService;
        private readonly StudentService studentService;

        public string UserId { get; set; }
        public IList<Grade> Grades { get; set; }

        public IndexModel(IHttpContextAccessor httpContextAccessor, GradeService gradeService, StudentService studentService)
        {
            _httpContextAccessor = httpContextAccessor;
            this.gradeService = gradeService;
            this.studentService = studentService;
            UserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public async Task OnGetAsync()
        {
            int studentId = await studentService.GetStudentId(UserId);
            Grades = await gradeService.GetAllGradesByStudentAsync(studentId);
        }
    }
}
