using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SchoolGradebook.Models;
using SchoolGradebook.Data;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Teachers
{
    public class CreateModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly SubjectService subjectService;
        private readonly ApprobationService approbationService;

        public CreateModel(TeacherService teacherService, SubjectService subjectService, ApprobationService approbationService)
        {
            this.teacherService = teacherService;
            this.subjectService = subjectService;
            this.approbationService = approbationService;
            Approbations = new List<int>();
        }

        public async Task<IActionResult> OnGetAsync()
        {
            SubjectTypes = await subjectService.GetAllSubjectTypesAsync();
            return Page();
        }
        public List<SubjectType> SubjectTypes { get; set; }
        [BindProperty]
        public List<int> Approbations { get; set; }
        [BindProperty]
        public Models.Teacher Teacher { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await teacherService.AddTeacherAsync(Teacher, Approbations);
            return RedirectToPage("./Index");
        }
    }
}
