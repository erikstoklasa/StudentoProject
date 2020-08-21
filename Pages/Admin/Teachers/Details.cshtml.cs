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
    public class DetailsModel : PageModel
    {
        private readonly TeacherService teacherService;
        private readonly ApprobationService approbationService;

        public DetailsModel(TeacherService teacherService, ApprobationService approbationService)
        {
            this.teacherService = teacherService;
            this.approbationService = approbationService;
        }

        public Models.Teacher Teacher { get; set; }
        public List<Approbation> Approbations { get; set; }
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

            Approbations = await approbationService.GetAllApprobations(Teacher.Id);

            return Page();
        }
    }
}
