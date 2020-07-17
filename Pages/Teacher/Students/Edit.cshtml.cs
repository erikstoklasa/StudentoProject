using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Students
{
    public class EditModel : PageModel
    {
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;

        public string UserId { get; set; }
        public int TeacherId { get; set; }

        public EditModel(IHttpContextAccessor httpContextAccessor, TeacherAccessValidation teacherAccessValidation, TeacherService teacherService, StudentService studentService)
        {
            this.teacherAccessValidation = teacherAccessValidation;
            this.teacherService = teacherService;
            this.studentService = studentService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }

        [BindProperty]
        public Models.Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Student = await studentService.GetStudentAsync((int)id);
            if (Student == null)
            {
                return NotFound();
            }

            TeacherId = await teacherService.GetTeacherId(UserId);
            bool teacherHasAccessToStudent = await teacherAccessValidation.HasAccessToStudent(TeacherId, Student.Id);
            if (!teacherHasAccessToStudent)
            {
                return BadRequest();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Student == null)
            {
                return NotFound();
            }

            Models.Student student = await studentService.GetStudentAsync(Student.Id);
            Student.UserAuthId = student.UserAuthId;
            //Preventing from teacher overposting UserAuthId and changing it

            TeacherId = await teacherService.GetTeacherId(UserId);
            bool teacherHasAccessToStudent = await teacherAccessValidation.HasAccessToStudent(TeacherId, Student.Id);

            if (!teacherHasAccessToStudent)
            {
                return BadRequest();
            }

            bool updatedSuccessfully = await studentService.UpdateStudent(Student);
            if (!updatedSuccessfully)
            {
                ViewData["status_type"] = "error";
                ViewData["status_message"] = "Nevyplnili jste všechny údaje správně.";
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
