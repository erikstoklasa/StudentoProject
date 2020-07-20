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
