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

namespace SchoolGradebook.Pages.Admin.Subjects
{
    public class CreateModel : PageModel
    {
        public List<SelectListItem> TeacherSelectList { get; set; }
        public List<SelectListItem> SubjectTypeSelectList { get; set; }
        private readonly SubjectService subjectService;
        private readonly TeacherService teacherService;

        public CreateModel(SubjectService subjectService, TeacherService teacherService)
        {
            this.subjectService = subjectService;
            this.teacherService = teacherService;
            TeacherSelectList = new List<SelectListItem>();
            SubjectTypeSelectList = new List<SelectListItem>();
        }

        public async Task<IActionResult> OnGet()
        {
            foreach (Models.Teacher t in await teacherService.GetAllTeachersAsync())
            {
                TeacherSelectList.Add(new SelectListItem() { Text = t.GetFullName(), Value = t.Id.ToString() });
            }
            foreach (SubjectType st in await subjectService.GetAllSubjectTypesAsync())
            {
                SubjectTypeSelectList.Add(new SelectListItem() { Text = st.Name, Value = st.Id.ToString() });
            }
            return Page();
        }

        [BindProperty]
        public SubjectInstance SubjectInstance { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            await subjectService.AddSubjectInstanceAsync(SubjectInstance);

            return RedirectToPage("./Index");
        }
    }
}
