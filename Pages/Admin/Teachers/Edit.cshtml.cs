using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Services;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Teachers
{
    public class EditModel : PageModel
    {
        private readonly TeacherService teacherService;

        public EditModel(TeacherService teacherService)
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

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Models.Teacher t = await teacherService.GetTeacherAsync(Teacher.Id);
            Teacher.UserAuthId = t.UserAuthId;

            if(await teacherService.UpdateTeacherAsync(Teacher)){
                return RedirectToPage("./Index");
            }
            //TODO: Return message to user when the updating doesn't pass
            return Page();
        }
    }
}
