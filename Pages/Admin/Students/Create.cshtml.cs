using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class CreateModel : PageModel
    {
        private readonly StudentService studentService;

        public CreateModel(StudentService studentService)
        {
            this.studentService = studentService;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Models.Student Student { get; set; }

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
                ViewData["status_message"] = $"{Student.getFullName()} byl přidán do matriky.";
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
