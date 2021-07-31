using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class DeleteModel : PageModel
    {
        private readonly StudentService studentService;

        public DeleteModel(StudentService studentService)
        {
            this.studentService = studentService;
        }

        [BindProperty]
        public Models.Student Student { get; set; }

        public string ErrorMessage { get; set; }
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
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            List<int> studentIdsToDelete = new();
            studentIdsToDelete.Add((int)id);
            try
            {

            await studentService.RemoveStudentsAsync(studentIdsToDelete);
            }
            catch (Exception)
            {
                ErrorMessage = "Bohožel se nepodařilo odstranit daného studenta, pro odstranění kontaktujte prosím podporu";
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
