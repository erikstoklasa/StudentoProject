using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Teacher.Classes
{
    public class DetailsModel : PageModel
    {
        private readonly ClassService classService;
        private readonly StudentService studentService;
        public List<Models.Student> Students { get; set; }

        public DetailsModel(ClassService classService, StudentService studentService)
        {
            this.classService = classService;
            this.studentService = studentService;
        }

        public Class Class { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Class = await classService.GetClassAsync((int)id);

            if (Class == null)
            {
                return NotFound();
            }
            Students = await studentService.GetAllStudentsByClassAsync((int)id);
            return Page();
        }
    }
}
