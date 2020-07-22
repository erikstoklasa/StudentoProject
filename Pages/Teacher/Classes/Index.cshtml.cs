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

namespace SchoolGradebook.Pages.Teacher.Classes
{
    public class IndexModel : PageModel
    {
        private readonly ClassService classService;

        public IndexModel(ClassService classService)
        {
            this.classService = classService;
        }

        public IList<Class> Classes { get;set; }

        public async Task OnGetAsync()
        {
            Classes = await classService.GetAllClasses();
        }
    }
}
