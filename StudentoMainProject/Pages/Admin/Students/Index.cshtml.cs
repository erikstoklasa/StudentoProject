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

namespace SchoolGradebook.Pages.Admin.Students
{
    public class IndexModel : PageModel
    {
        private readonly StudentService studentService;

        public IndexModel(StudentService studentService)
        {
            this.studentService = studentService;
        }

        public List<Models.Student> Students { get;set; }

        public async Task OnGetAsync()
        {
            Students = (await studentService.GetAllStudentsAsync()).ToList();
        }
    }
}
