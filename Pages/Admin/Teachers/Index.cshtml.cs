using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Data;
using SchoolGradebook.Models;
using SchoolGradebook.Services;

namespace SchoolGradebook.Pages.Admin.Teachers
{
    public class IndexModel : PageModel
    {
        private readonly TeacherService teacherService;

        public IndexModel(TeacherService teacherService)
        {
            this.teacherService = teacherService;
        }

        public IList<Models.Teacher> Teachers { get;set; }

        public async Task OnGetAsync()
        {
            Teachers = await teacherService.GetAllTeachersAsync();
        }
    }
}
