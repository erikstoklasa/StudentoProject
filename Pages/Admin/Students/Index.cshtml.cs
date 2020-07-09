using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SchoolGradebook.Models;
using SchoolGradebook.Data;

namespace SchoolGradebook.Pages.Admin.Students
{
    public class IndexModel : PageModel
    {
        private readonly SchoolGradebook.Data.SchoolContext _context;

        public IndexModel(SchoolGradebook.Data.SchoolContext context)
        {
            _context = context;
        }

        public IList<Models.Student> Student { get;set; }

        public async Task OnGetAsync()
        {
            Student = await _context.Students.ToListAsync();
        }
    }
}
