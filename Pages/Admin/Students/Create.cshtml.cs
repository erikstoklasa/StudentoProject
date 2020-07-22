using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class CreateModel : PageModel
    {
        private readonly StudentService studentService;
        private readonly ClassService classService;

        public CreateModel(StudentService studentService, ClassService classService)
        {
            this.studentService = studentService;
            this.classService = classService;
            ClassesList = new List<SelectListItem>();
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
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
