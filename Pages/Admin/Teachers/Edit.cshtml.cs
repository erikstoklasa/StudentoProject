using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SchoolGradebook.Models;
using SchoolGradebook.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchoolGradebook.Pages.Admin.Teachers
{
    public class EditModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly ApprobationService approbationService;
        private readonly SubjectService subjectService;

        public EditModel(TeacherService teacherService, ApprobationService approbationService, SubjectService subjectService)
        {
            this.teacherService = teacherService;
            this.approbationService = approbationService;
            this.subjectService = subjectService;
            Approbations = new List<int>();
        }

        [BindProperty]
        public Models.Teacher Teacher { get; set; }
        [BindProperty]
        public List<int> Approbations { get; set; }
        public List<SubjectType> AllSubjectTypes { get; set; }

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
            AllSubjectTypes = await subjectService.GetAllSubjectTypesAsync();
            List<Approbation> apprs = await approbationService.GetAllApprobations((int)id);
            foreach(var a in apprs)
            {
                Approbations.Add(a.SubjectTypeId);
            }
            return Page();
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            Models.Teacher t = await teacherService.GetTeacherAsync(Teacher.Id);
            Teacher.UserAuthId = t.UserAuthId;

            if (await teacherService.UpdateTeacherAsync(Teacher, Approbations))
            {
                return RedirectToPage("./Index");
            }
            //TODO: Return message to user when the updating doesn't pass
            return Page();
        }
    }
}
