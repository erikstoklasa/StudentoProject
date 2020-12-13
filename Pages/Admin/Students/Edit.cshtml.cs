using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class EditModel : PageModel
    {
        private readonly ClassService classService;
        private readonly StudentService studentService;
        private readonly AdminService adminService;

        public EditModel(ClassService classService, StudentService studentService, IHttpContextAccessor httpContextAccessor, AdminService adminService)
        {
            this.classService = classService;
            this.studentService = studentService;
            this.adminService = adminService;
            ClassesList = new List<SelectListItem>();
            UserId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserId ??= "";
        }

        [BindProperty]
        public Models.Student Student { get; set; }
        public List<SelectListItem> ClassesList { get; set; }
        public List<Class> Classes { get; set; }
        public string UserId { get; set; }

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
            Classes = await classService.GetAllClasses();
            foreach (Class c in Classes)
            {
                ClassesList.Add(new SelectListItem(c.GetName(), c.Id.ToString()));
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Models.Student s = await studentService.GetStudentFullProfileAsync((int)Student.Id);
            Student.UserAuthId = s.UserAuthId;
            int adminId = await adminService.GetAdminId(UserId);
            Student.SchoolId = (await adminService.GetAdminById(adminId)).SchoolId;

            if (!ModelState.IsValid)
            {
                Classes = await classService.GetAllClasses();
                foreach (Class c in Classes)
                {
                    ClassesList.Add(new SelectListItem(c.GetName(), c.Id.ToString()));
                }
                return Page();
            }

            if (await studentService.UpdateStudentAsync(Student))
            {
                ViewData["status_type"] = "success";
                ViewData["status_message"] = $"Student {Student.GetFullName()} byl upraven";
            }
            else
            {
                ViewData["status_type"] = "error";
                ViewData["status_message"] = "Nevyplnili jste všechny nutné údaje správně";
            }
            Student = s; //Copying for redisplaying data to the user
            Classes = await classService.GetAllClasses();
            foreach (Class c in Classes)
            {
                ClassesList.Add(new SelectListItem(c.GetName(), c.Id.ToString()));
            }
            return Page();
        }
    }
}
