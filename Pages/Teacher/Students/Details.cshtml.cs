using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Services;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Students
{
    public class DetailsModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly StudentService studentService;

        public string UserId { get; set; }
        public int TeacherId { get; set; }

        public DetailsModel(IHttpContextAccessor httpContextAccessor, TeacherService teacherService, TeacherAccessValidation teacherAccessValidation, StudentService studentService)
        {
            this.teacherService = teacherService;
            this.teacherAccessValidation = teacherAccessValidation;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
        }

        public Models.Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Student = await studentService.GetStudentFullProfileAsync((int)id);

            if (Student == null)
            {
                return NotFound();
            }

            TeacherId = await teacherService.GetTeacherId(UserId);
            bool hasAccessToStudent = await teacherAccessValidation.HasAccessToStudent(TeacherId, Student.Id);
            if (!hasAccessToStudent)
            {
                return BadRequest();
            }

            return Page();
        }
    }
}
