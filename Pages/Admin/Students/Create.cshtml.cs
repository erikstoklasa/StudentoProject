using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class CreateModel : PageModel
    {
        private readonly StudentService studentService;
        private readonly ClassService classService;
        private readonly AdminService adminService;

        public string UserId { get; set; }

        public CreateModel(StudentService studentService, ClassService classService, IHttpContextAccessor httpContextAccessor, AdminService adminService)
        {
            this.studentService = studentService;
            this.classService = classService;
            this.adminService = adminService;
            ClassesList = new List<SelectListItem>();
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        public async Task<IActionResult> OnGet()
        {
            Classes = await classService.GetAllClasses();
            foreach (Models.Class c in Classes)
            {
                ClassesList.Add(new SelectListItem(c.GetName(), c.Id.ToString()));
            }
            return Page();
        }

        [BindProperty]
        public Models.Student Student { get; set; }
        public List<SelectListItem> ClassesList { get; set; }
        public List<Models.Class> Classes { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            int adminId = await adminService.GetAdminId(UserId);
            Student.SchoolId = (await adminService.GetAdminById(adminId)).SchoolId;
            if (!ModelState.IsValid)
            {
                ViewData["status_type"] = "error";
                ViewData["status_message"] = "Error, nevložili jste správně všechny požadované údaje.";
                return Page();
            }
            if (await studentService.AddStudentAsync(Student))
            {
                ViewData["status_type"] = "success";
                ViewData["status_message"] = $"{Student.GetFullName()} byl přidán do matriky.";
                return Page();
            }
            else
            {
                ViewData["status_type"] = "error";
                ViewData["status_message"] = "Error, nevložili jste správně všechny požadované údaje.";
                return Page();
            }
        }
    }
}
