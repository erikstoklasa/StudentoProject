using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Classes
{
    public class CreateModel : PageModel
    {
        private readonly ClassService classService;
        private readonly TeacherService teacherService;

        public List<SelectListItem> TeachersList { get; set; }
        public List<Models.Teacher> Teachers { get; set; }

        public CreateModel(ClassService classService, TeacherService teacherService)
        {
            this.classService = classService;
            this.teacherService = teacherService;
            TeachersList = new List<SelectListItem>();
        }

        public async Task<IActionResult> OnGet()
        {
            Teachers = (await teacherService.GetAllTeachersAsync()).ToList();
            foreach(Models.Teacher t in Teachers)
            {
                TeachersList.Add(new SelectListItem(t.GetFullName(), t.Id.ToString()));
            }

            return Page();
        }

        [BindProperty]
        public Class Class { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await classService.AddClassAsync(Class);

            return RedirectToPage("./Index");
        }
    }
}
