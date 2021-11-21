using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Classes
{
    public class DeleteModel : PageModel
    {
        private readonly ClassService classService;
        private readonly StudentService studentService;

        public DeleteModel(ClassService classService, StudentService studentService)
        {
            this.classService = classService;
            this.studentService = studentService;
        }

        [BindProperty]
        public Class Class { get; set; }
        public List<Models.Student> Students { get; set; }

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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            List<int> studentIdsToDelete = await studentService.GetAllStudentIdsByClassAsync((int)id);
            await studentService.RemoveStudentsAsync(studentIdsToDelete);
            await classService.DeleteClassAsync((int)id);

            return RedirectToPage("./Index");
        }
    }
}
