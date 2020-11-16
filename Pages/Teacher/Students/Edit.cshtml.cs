using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Students
{
    public class EditModel : PageModel
    {
        private readonly TeacherAccessValidation teacherAccessValidation;
        private readonly TeacherService teacherService;
        private readonly StudentService studentService;
        private readonly ClassService classService;

        public string UserId { get; set; }
        public int TeacherId { get; set; }

        public EditModel(IHttpContextAccessor httpContextAccessor, TeacherAccessValidation teacherAccessValidation, TeacherService teacherService, StudentService studentService, ClassService classService)
        {
            this.teacherAccessValidation = teacherAccessValidation;
            this.teacherService = teacherService;
            this.studentService = studentService;
            this.classService = classService;
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ClassesList = new List<SelectListItem>();
        }

        [BindProperty]
        public Models.Student Student { get; set; }
        public List<SelectListItem> ClassesList { get; set; }
        public List<Class> Classes { get; set; }

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
            bool teacherHasAccessToStudent = await teacherAccessValidation.HasAccessToStudent(TeacherId, (int)id);
            if (!teacherHasAccessToStudent)
            {
                return Forbid();
            }
            Classes = await classService.GetAllClasses();
            foreach (Class c in Classes)
            {
                ClassesList.Add(new SelectListItem(c.GetName(), c.Id.ToString()));
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

            //Preventing from teacher overposting UserAuthId and changing it
            Models.Student student = await studentService.GetStudentAsync(Student.Id);
            Student.UserAuthId = student.UserAuthId;

            //Preventing from teacher overposting SchoolId and changing it
            Student.SchoolId = student.SchoolId;

            TeacherId = await teacherService.GetTeacherId(UserId);
            bool teacherHasAccessToStudent = await teacherAccessValidation.HasAccessToStudent(TeacherId, Student.Id);

            if (!teacherHasAccessToStudent)
            {
                return BadRequest();
            }

            bool updatedSuccessfully = await studentService.UpdateStudentAsync(Student);
            Student = await studentService.GetStudentFullProfileAsync(Student.Id);
            Classes = await classService.GetAllClasses();
            foreach (Class c in Classes)
            {
                ClassesList.Add(new SelectListItem(c.GetName(), c.Id.ToString()));
            }
            if (!updatedSuccessfully)
            {
                ViewData["status_type"] = "error";
                ViewData["status_message"] = "Nevyplnili jste všechny údaje správně.";
            }
            else
            {
                ViewData["status_type"] = "success";
                ViewData["status_message"] = "Profil byl úspěšně upraven.";
            }

            return Page();
        }
    }
}
