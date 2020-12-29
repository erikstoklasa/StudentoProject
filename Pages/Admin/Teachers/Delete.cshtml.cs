using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;
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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Teacher = await teacherService.GetTeacherAsync((int)id);

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

            await teacherService.DeleteTeacherAsync((int)id);

            return RedirectToPage("./Index");
        }
    }
}
