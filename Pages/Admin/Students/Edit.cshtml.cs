using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class EditModel : PageModel
    {
        private readonly ClassService classService;
        private readonly StudentService studentService;

        public EditModel(ClassService classService, StudentService studentService)
        {
            this.classService = classService;
            this.studentService = studentService;
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

            Student = await studentService.GetStudentAsync((int)id);
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Models.Student s = await studentService.GetStudentAsync((int)Student.Id);
            Student.UserAuthId = s.UserAuthId;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            if(await studentService.UpdateStudentAsync(Student))
            {
                ViewData["status_type"] = "success";
                ViewData["status_message"] = $"Student {Student.GetFullName()} byl upraven";
            } else
            {
                ViewData["status_type"] = "error";
                ViewData["status_message"] = "Nevyplnili jste všechny nutné údaje správně";
            }

            return Page();
        }
    }
}
