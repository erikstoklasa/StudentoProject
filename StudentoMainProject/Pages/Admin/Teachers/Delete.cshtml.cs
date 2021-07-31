using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;
using System;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Teachers
{
    public class DeleteModel : PageModel
    {
        private readonly TeacherService teacherService;

        public DeleteModel(TeacherService teacherService)
        {
            this.teacherService = teacherService;
        }

        [BindProperty]
        public Models.Teacher Teacher { get; set; }
        public string ErrorMessage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Teacher = await teacherService.GetTeacherFullProfileAsync((int)id);

            if (Teacher == null)
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
            try
            {
                await teacherService.DeleteTeacherAsync((int)id);
            }
            catch (Exception)
            {
                ErrorMessage = "Bohožel se nepodařilo odstranit daného vyučujícího, pro odstranění kontaktujte prosím podporu";
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
